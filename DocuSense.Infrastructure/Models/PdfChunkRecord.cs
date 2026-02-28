using Microsoft.Extensions.VectorData;

namespace DocuSense.Infrastructure.Models
{
    public class PdfChunkRecord
    {
        [VectorStoreKey]
        public string Id { get; set; }

        [VectorStoreData]
        public string Text { get; set; }

        [VectorStoreVector(1536)]
        public ReadOnlyMemory<float> Vector { get; set; }
    }
}