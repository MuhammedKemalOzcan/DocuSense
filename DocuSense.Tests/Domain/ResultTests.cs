using DocuSense.Domain.Errors;

namespace DocuSense.Tests.Domain
{
    public class ResultTests
    {
        [Fact]
        public void Success_ShouldSetIsSuccessToTrue()
        {
            var result = Result<string>.Success("Test Data");

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Success_ShouldContainCorrectData()
        {
            var result = Result<int>.Success(42);

            Assert.Equal(42, result.Data);
        }

        [Fact]
        public void Success_ShouldHaveNoError()
        {
            var result = Result<string>.Success("test");

            Assert.Equal(Error.None, result.Error);
        }

        [Fact]
        public void Failure_ShouldSetIsSuccessToFalse()
        {
            var error = Error.Validation("Test.Error", "Bir Hata Oluştu");

            var result = Result<string>.Failure(error);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Failure_ShouldContainCorrectError()
        {
            var error = Error.Validation("Test.Error", "Bir Hata Oluştu");
            var result = Result<int>.Failure(error);

            Assert.Equal("Test.Error", result.Error.Code);
            Assert.Equal("Bir Hata Oluştu", result.Error.Message);
        }

        [Fact]
        public void Failure_ShouldHaveDefaultValue()
        {
            var error = Error.Validation("Test.Error", "Bir Hata Oluştu");

            var result = Result<string>.Failure(error);

            Assert.Null(result.Data);
        }

        // Farklı Error tipleri doğru Type taşımalı
        [Fact]
        public void Failure_WithDifferentErrorTypes_ShouldHaveCorrectType()
        {
            // ARRANGE & ACT
            var validationResult = Result<string>.Failure(
                Error.Validation("V1", "Validation hatası"));
            var notFoundResult = Result<string>.Failure(
                Error.NotFound("N1", "Bulunamadı"));
            var unauthorizedResult = Result<string>.Failure(
                Error.Unauthorized("U1", "Yetkisiz"));

            // ASSERT
            Assert.Equal(ErrorType.Validation, validationResult.Error.Type);
            Assert.Equal(ErrorType.NotFound, notFoundResult.Error.Type);
            Assert.Equal(ErrorType.Unauthorized, unauthorizedResult.Error.Type);
        }


    }
}