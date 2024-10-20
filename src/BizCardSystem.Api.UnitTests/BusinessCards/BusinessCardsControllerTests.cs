using BizCardSystem.Api.Controllers;
using BizCardSystem.Application.BusinessCards.Dtos.Create;
using BizCardSystem.Application.BusinessCards.Dtos.File;
using BizCardSystem.Application.BusinessCards.Dtos.Get;
using BizCardSystem.Application.Repositories;
using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Domain.BusinessCards.Filters;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace BizCardSystem.Api.UnitTests.BusinessCards;

public class BusinessCardsControllerTests
{
    private readonly IBusinessCardsService _mockService;
    private readonly IValidator<CreateBizRequest> _mockCreateValidator;
    private readonly IValidator<CreateFromFileRequest> _mockFileValidator;
    private readonly BusinessCardsController _controller;

    public BusinessCardsControllerTests()
    {
        _mockService = Substitute.For<IBusinessCardsService>();
        _mockCreateValidator = Substitute.For<IValidator<CreateBizRequest>>();
        _mockFileValidator = Substitute.For<IValidator<CreateFromFileRequest>>();

        _controller = new BusinessCardsController(_mockService);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithBusinessCards()
    {
        // Arrange
        var filter = new BusinessCardFilter();
        var businessCards = new List<GetBizResponse> { new GetBizResponse() };

        var resultbusinessCards = ListResult<GetBizResponse>.Success(businessCards);
        resultbusinessCards.TotalRecord = businessCards.Count;
        _mockService.GetAllAsync(filter).Returns(Task.FromResult(resultbusinessCards));

        // Act
        var result = await _controller.GetAll(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(businessCards, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WithBusinessCard()
    {
        // Arrange
        var id = 1;
        var businessCard = new GetBizResponse();
        _mockService.GetByIdAsync(id).Returns(Result.Success(businessCard));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(businessCard, okResult.Value);
    }

    [Fact]
    public async Task Create_ShouldReturnValidationProblem_WhenValidationFails()
    {
        // Arrange
        var createRequest = new CreateBizRequest();
        var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Error") });
        _mockCreateValidator.ValidateAsync(createRequest, default).Returns(validationResult);

        // Act
        var result = await _controller.Create(createRequest, _mockCreateValidator);

        // Assert
        var validationProblem = Assert.IsType<ValidationProblemDetails>(result);
        Assert.Contains("Name", validationProblem.Errors.Keys);
    }

    [Fact]
    public async Task Create_ShouldReturnOkResult_WithCreatedBusinessCard()
    {
        // Arrange
        var createRequest = new CreateBizRequest();
        var validationResult = new ValidationResult();
        var createdBusinessCardId = 1;
        _mockCreateValidator.ValidateAsync(createRequest, default).Returns(validationResult);
        _mockService.CreateAsync(createRequest).Returns(Result.Success(createdBusinessCardId));

        // Act
        var result = await _controller.Create(createRequest, _mockCreateValidator);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(createdBusinessCardId, okResult.Value);
    }

    [Fact]
    public async Task CreateBusinessCardByFileAsync_ShouldReturnValidationProblem_WhenValidationFails()
    {
        // Arrange
        var fileRequest = new CreateFromFileRequest();
        var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("File", "Error") });
        _mockFileValidator.ValidateAsync(fileRequest, default).Returns(validationResult);

        // Act
        var result = await _controller.CreateBusinessCardByFileAsync(fileRequest, _mockFileValidator);

        // Assert
        var validationProblem = Assert.IsType<ValidationProblemDetails>(result);
        Assert.Contains("File", validationProblem.Errors.Keys);
    }

    [Fact]
    public async Task CreateBusinessCardByFileAsync_ShouldReturnOkResult_WithCreatedBusinessCard()
    {
        // Arrange
        var fileRequest = new CreateFromFileRequest();
        var validationResult = new ValidationResult();
        var createdBusinessCard = new CreateBizRequest();
        _mockFileValidator.ValidateAsync(fileRequest, default).Returns(validationResult);
        _mockService.CreateBusinessCardByFileAsync(fileRequest.File).Returns(Result.Success(createdBusinessCard));

        // Act
        var result = await _controller.CreateBusinessCardByFileAsync(fileRequest, _mockFileValidator);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(createdBusinessCard, okResult.Value);
    }

    [Fact]
    public async Task DeleteById_ShouldReturnOkResult_WithDeletionStatus()
    {
        // Arrange
        var id = 1;
        _mockService.DeleteAsync(id).Returns(Result.Success(id));

        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(id, okResult.Value);
    }

    [Fact]
    public async Task ExportToCsv_ShouldReturnFileResult_WithCsvContent()
    {
        // Arrange
        var id = 1;
        var fileContent = new byte[] { 0x00 };
        _mockService.ExportToCsv(id).Returns(Result.Success(fileContent));

        // Act
        var result = await _controller.ExportToCsv(id);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);
        Assert.Equal("business_card.csv", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task ExportToXml_ShouldReturnFileResult_WithXmlContent()
    {
        // Arrange
        var id = 1;
        var fileContent = new byte[] { 0x00 };
        _mockService.ExportToXml(id).Returns(Result.Success(fileContent));

        // Act
        var result = await _controller.ExportToXml(id);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/xml", fileResult.ContentType);
        Assert.Equal("business_card.xml", fileResult.FileDownloadName);
    }
}