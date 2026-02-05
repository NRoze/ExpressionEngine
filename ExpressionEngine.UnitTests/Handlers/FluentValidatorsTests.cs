using Moq;
using ExpressionEngine.Api.Validators;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Shared.DTOs;
using ExpressionEngine.Shared.Enums;
using FluentValidation;

namespace ExpressionEngine.UnitTests.Handlers
{
    public class FluentValidatorsTests
    {
        [Fact]
        public async Task CreateOperationValidator_ValidDto_PassesValidation_And_CallsExpressionValidator()
        {
            var mockExprValidator = new Mock<IExpressionValidator>();
            mockExprValidator
                .Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<OperationType>()))
                .ReturnsAsync(true);

            var sut = new CreateOperationValidator(mockExprValidator.Object);

            var dto = new CreateOperationDto("MyOp", "A + B", OperationType.Numeric);

            var result = await sut.ValidateAsync(dto);

            Assert.True(result.IsValid);
            mockExprValidator.Verify(x => x.ValidateAsync("A + B", OperationType.Numeric), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateOperationValidator_NameEmpty_FailsValidation(string name)
        {
            var mockExprValidator = new Mock<IExpressionValidator>();
            mockExprValidator
                .Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<OperationType>()))
                .ReturnsAsync(true);

            var sut = new CreateOperationValidator(mockExprValidator.Object);

            var dto = new CreateOperationDto(name!, "A+B", OperationType.Numeric);

            var result = await sut.ValidateAsync(dto);

            Assert.False(result.IsValid);
            var nameErrors = result.Errors.Where(e => e.PropertyName == "Name").ToList();
            Assert.Contains(nameErrors, e => e.ErrorMessage == "Name is required");
        }

        [Fact]
        public async Task CreateOperationValidator_NameTooLong_FailsValidation()
        {
            var mockExprValidator = new Mock<IExpressionValidator>();
            mockExprValidator.Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<OperationType>()))
                             .ReturnsAsync(true);

            var sut = new CreateOperationValidator(mockExprValidator.Object);

            var longName = new string('x', 101);
            var dto = new CreateOperationDto(longName, "A+B", OperationType.Numeric);

            var result = await sut.ValidateAsync(dto);

            Assert.False(result.IsValid);
            var nameErrors = result.Errors.Where(e => e.PropertyName == "Name").ToList();
            Assert.Contains(nameErrors, e => e.ErrorMessage == "Name cannot exceed 100 characters");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateOperationValidator_ExpressionEmpty_FailsValidation(string expr)
        {
            var mockExprValidator = new Mock<IExpressionValidator>();
            mockExprValidator.Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<OperationType>()))
                             .ReturnsAsync(true);

            var sut = new CreateOperationValidator(mockExprValidator.Object);

            var dto = new CreateOperationDto("Op", expr!, OperationType.Numeric);

            var result = await sut.ValidateAsync(dto);

            Assert.False(result.IsValid);
            var exprErrors = result.Errors.Where(e => e.PropertyName == "Expression").ToList();
            Assert.Contains(exprErrors, e => e.ErrorMessage == "Expression is required");
        }

        [Fact]
        public async Task CreateOperationValidator_ExpressionTooLong_FailsValidation()
        {
            var mockExprValidator = new Mock<IExpressionValidator>();
            mockExprValidator.Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<OperationType>()))
                             .ReturnsAsync(true);

            var sut = new CreateOperationValidator(mockExprValidator.Object);

            var longExpr = new string('x', 501);
            var dto = new CreateOperationDto("Op", longExpr, OperationType.String);

            var result = await sut.ValidateAsync(dto);

            Assert.False(result.IsValid);
            var exprErrors = result.Errors.Where(e => e.PropertyName == "Expression").ToList();
            Assert.Contains(exprErrors, e => e.ErrorMessage == "Expression cannot exceed 500 characters");
        }

        [Fact]
        public async Task CreateOperationValidator_InvalidExpressionFormat_FailsMustAsyncRule()
        {
            var mockExprValidator = new Mock<IExpressionValidator>();
            mockExprValidator
                .Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<OperationType>()))
                .ReturnsAsync(false);

            var sut = new CreateOperationValidator(mockExprValidator.Object);

            var dto = new CreateOperationDto("Op", "A + B", OperationType.Numeric);

            var result = await sut.ValidateAsync(dto);

            Assert.False(result.IsValid);
            var exprErrors = result.Errors.Where(e => e.PropertyName == "Expression").ToList();
            Assert.Contains(exprErrors, e => e.ErrorMessage == "Invalid expression format or unsupported tokens");

            mockExprValidator.Verify(x => x.ValidateAsync("A + B", OperationType.Numeric), Times.Once);
        }
    }
}
