using DocuSense.Application.Dtos;
using DocuSense.Application.Services;
using DocuSense.Infrastructure.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace DocuSense.Infrastructure.Services
{
    internal class VectorDatabaseService : IVectorDatabaseService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private readonly VectorStoreCollection<string, PdfChunkRecord> _collection;

        public VectorDatabaseService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, VectorStore vectorStore)
        {
            _embeddingGenerator = embeddingGenerator;
            _collection = vectorStore.GetCollection<string, PdfChunkRecord>("pdf_chunk_records");
        }

        public async Task IngestDataAsync(List<ChunkMetadataDto> chunks, string documentId)
        {
            var validChunks = chunks.Where(c => !string.IsNullOrWhiteSpace(c.Text)).ToList();

            if (validChunks.Count == 0)
            {
                // Eğer okunabilir hiçbir metin yoksa (Örn: Resim PDF'i) işlemi kibarca durdur
                throw new Exception("Bu belgeden okunabilir bir metin çıkarılamadı. Lütfen metin tabanlı bir PDF yükleyin.");
            }

            //Koleksiyonun varlığını kontrol et.
            await _collection.EnsureCollectionExistsAsync();

            //Metinleri içerisinden çıkartıyoruz
            var textsToEmbed = validChunks.Select(c => c.Text).ToList();

            //Chunkları Vektöre çeviriyor.
            var embedding = await _embeddingGenerator.GenerateAsync(textsToEmbed);

            for (int i = 0; i < validChunks.Count; i++)
            {
                var pdfChunkRecord = new PdfChunkRecord()
                {
                    Id = Guid.NewGuid().ToString(),
                    DocumentId = documentId,
                    Text = chunks[i].Text,
                    PageNumber = chunks[i].PageNumber,
                    Vector = embedding[i].Vector
                };

                await _collection.UpsertAsync(pdfChunkRecord);
            }
        }

        public async Task<List<string>> SearchDataAsync(string query, int top, string documentId)
        {
            //aranılan metni vektöre dönüştürür.
            ReadOnlyMemory<float> searchVector = (await _embeddingGenerator.GenerateAsync(query)).Vector;

            var vectorSearchOptions = new VectorSearchOptions<PdfChunkRecord>
            {
                Filter = r => r.DocumentId == documentId
            };

            //SearchAsync metoduyla vektöre dönüştürülen metnin benzerliğini bulur ve top kadar benzerlik getirir.
            List<VectorSearchResult<PdfChunkRecord>> searchResult = await _collection.SearchAsync(searchVector, top, vectorSearchOptions)
                .ToListAsync();

            List<string> context = [];
            foreach (var result in searchResult)
            {
                //Yapay zekaya gönderirken sayfa numarasını gönderiyoruz.
                string formattedContext = $"[Sayfa: {result.Record.PageNumber}] {result.Record.Text}";
                context.Add(formattedContext);
            }

            return context;
        }
    }
}