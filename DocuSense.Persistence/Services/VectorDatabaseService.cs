using DocuSense.Application.Services;
using DocuSense.Infrastructure.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace DocuSense.Persistence.Services
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

        public async Task IngestDataAsync(List<string> chunks)
        {
            var embedding = await _embeddingGenerator.GenerateAsync(chunks);
            await _collection.EnsureCollectionExistsAsync();

            for (int i = 0; i < chunks.Count; i++)
            {
                var pdfChunkRecord = new PdfChunkRecord()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = chunks[i],
                    Vector = embedding[i].Vector
                };

                await _collection.UpsertAsync(pdfChunkRecord);
            }
        }

        public async Task<List<string>> SearchDataAsync(string query, int top)
        {
            ReadOnlyMemory<float> searchVector = (await _embeddingGenerator.GenerateAsync(query)).Vector;

            List<VectorSearchResult<PdfChunkRecord>> searchResult = await _collection.SearchAsync(searchVector, top).ToListAsync();

            List<string> context = [];
            foreach (var result in searchResult)
            {
                context.Add(result.Record.Text);
            }

            return context;
        }
    }
}