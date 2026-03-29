using DocuSense.Application.Dtos;
using DocuSense.Application.Features.Queries.Commands.IngestData;
using DocuSense.Application.Services;
using DocuSense.Domain.Entites;
using DocuSense.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace DocuSense.Tests.Application.IngestData
{
    public class IngestDataCommandHandlerTests
    {
        private (
            Mock<IChatRepository> chatRepo,
            Mock<IPdfReaderService> pdfReader,
            Mock<ITextChunkerService> chunker,
            Mock<IUnitOfWork> unitOfWork,
            Mock<IVectorDatabaseService> vectorDb,
            Mock<IHttpContextAccessor> httpContext,
            IngestDataCommandHandler handler
        ) CreateHandler(string? userId = "test-user-id")
        {
            var chatRepo = new Mock<IChatRepository>();
            var pdfReader = new Mock<IPdfReaderService>();
            var chunker = new Mock<ITextChunkerService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var vectorDb = new Mock<IVectorDatabaseService>();
            var httpContext = new Mock<IHttpContextAccessor>();

            // HttpContext mock — userId varsa claim oluştur, yoksa boş bırak
            if (userId != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                var context = new DefaultHttpContext { User = principal };
                httpContext.Setup(x => x.HttpContext).Returns(context);
            }
            else
            {
                var context = new DefaultHttpContext();
                httpContext.Setup(x => x.HttpContext).Returns(context);
            }

            var handler = new IngestDataCommandHandler(
                chatRepo.Object, pdfReader.Object, chunker.Object,
                unitOfWork.Object, vectorDb.Object, httpContext.Object);

            return (chatRepo, pdfReader, chunker, unitOfWork, vectorDb, httpContext, handler);
        }

        private IFormFile CreateMockFile(string fileName = "test.pdf")
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(1024); // 1KB
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            return mockFile.Object;
        }

        [Fact]
        public async Task Handle_WhenUserUnauthorized_ShouldReturnFailure()
        {
            var (_, _, _, _, _, _, handler) = CreateHandler(userId: null);

            var command = new IngestDataCommand(CreateMockFile());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("User.Unauthorized", result.Error.Code);
        }

        [Fact]
        public async Task Handle_WhenFileIsNull_ShouldReturnFailure()
        {
            var (_, _, _, _, _, _, handler) = CreateHandler();

            var command = new IngestDataCommand(null);

            // ACT
            var result = await handler.Handle(command, CancellationToken.None);

            // ASSERT
            Assert.False(result.IsSuccess);
            Assert.Equal("File.NotFound", result.Error.Code);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ShouldReturnIngestResult()
        {
            // ARRANGE
            var (chatRepo, pdfReader, chunker, unitOfWork, vectorDb, _, handler) = CreateHandler();

            // PDF'den text çıkarma mock'u
            pdfReader.Setup(p => p.ExtractTextFromPdf(It.IsAny<string>()))
                .Returns(new List<ChunkMetadataDto>
                {
                    new ChunkMetadataDto { Text = "Bu bir test dokümanıdır." }
                });

            // Chunking mock'u
            chunker.Setup(c => c.ChunkText(It.IsAny<List<ChunkMetadataDto>>(), It.IsAny<int>()))
                .Returns(new List<ChunkMetadataDto>
                {
                    new ChunkMetadataDto { Text = "Bu bir test dokümanıdır." }
                });

            // Vector DB'ye kaydetme mock'u
            vectorDb.Setup(v => v.IngestDataAsync(It.IsAny<List<ChunkMetadataDto>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var command = new IngestDataCommand(CreateMockFile("rapor.pdf"));

            // ACT
            var result = await handler.Handle(command, CancellationToken.None);

            // ASSERT
            Assert.True(result.IsSuccess);
            Assert.Equal("rapor.pdf", result.Data.Title);
            Assert.NotNull(result.Data.DocumentId);

            // Repository'ye chat eklenmiş mi doğrula
            chatRepo.Verify(r => r.Add(It.IsAny<Chat>()), Times.Once);
            // SaveChanges çağrılmış mı doğrula
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}