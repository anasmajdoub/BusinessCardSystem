using AutoMapper;
using BizCardSystem.Application.Abstractions.Data;
using BizCardSystem.Application.BusinessCards.Dtos.Create;
using BizCardSystem.Application.BusinessCards.Dtos.Get;
using BizCardSystem.Application.Repositories;
using BizCardSystem.Application.Services;
using BizCardSystem.Domain.BusinessCards;
using BizCardSystem.Domain.BusinessCards.Errors;
using BizCardSystem.Domain.FileHelper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
namespace BizCardSystem.Application.UnitTests.BusinessCards;

public class BusinessCardsServiceTests
{
    private readonly IBusinessCardsRepository _mockRepository;
    private readonly ISqlConnectionFactory _mockSqlConnectionFactory;
    private readonly IMapper _mockMapper;
    private readonly IFileParserManager _mockFileParserManager;
    private readonly IValidator<BusinessCard> _mockValidator;
    private readonly BusinessCardsService _service;

    public BusinessCardsServiceTests()
    {
        _mockRepository = Substitute.For<IBusinessCardsRepository>();
        _mockSqlConnectionFactory = Substitute.For<ISqlConnectionFactory>();
        _mockMapper = Substitute.For<IMapper>();
        _mockFileParserManager = Substitute.For<IFileParserManager>();
        _mockValidator = Substitute.For<IValidator<BusinessCard>>();

        _service = new BusinessCardsService(
            _mockRepository,
            _mockSqlConnectionFactory,
            _mockMapper,
            _mockFileParserManager,
            _mockValidator
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenBusinessCardIsCreated()
    {
        // Arrange
        var createRequest = new CreateBizRequest();
        var businessCard = new BusinessCard { Id = 1 };
        _mockMapper.Map<BusinessCard>(createRequest).Returns(businessCard);
        _mockRepository.CreateAsync(businessCard).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(createRequest);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenBusinessCardNotFound()
    {
        // Arrange
        var id = 1;
        _mockRepository.GetByIdAsync(id).Returns((BusinessCard)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(BusinessCardErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenBusinessCardIsFound()
    {
        // Arrange
        var id = 1;
        var businessCard = new BusinessCard();
        var getBizResponse = new GetBizResponse();
        _mockRepository.GetByIdAsync(id).Returns(businessCard);
        _mockMapper.Map<GetBizResponse>(businessCard).Returns(getBizResponse);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(getBizResponse, result.Value);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenBusinessCardIsDeleted()
    {
        // Arrange
        var id = 1;
        _mockRepository.DeleteAsync(id).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value);
    }

    [Fact]
    public async Task CreateBusinessCardByFileAsync_ShouldReturnSuccess_WhenFileIsParsedSuccessfully()
    {
        // Arrange
        var file = Substitute.For<IFormFile>();
        file.OpenReadStream().Returns(new MemoryStream());

        var fileParsers = new List<FileParser> { new FileParser() };
        var bizResponses = new List<CreateBizRequest> { new CreateBizRequest() };
        var businessCards = new List<BusinessCard> { new BusinessCard { Email = "test@gmail.com" } };

        _mockFileParserManager.ParseFile(file).Returns(fileParsers);
        _mockMapper.Map<List<BusinessCard>>(fileParsers).Returns(businessCards);
        _mockMapper.Map<CreateBizRequest>(businessCards.First()).Returns(bizResponses.First());

        // Act
        var result = await _service.CreateBusinessCardByFileAsync(file);

        // Assert
        Assert.True(result.IsSuccess);
    }


    [Fact]
    public async Task ExportToCsv_ShouldReturnFailure_WhenBusinessCardNotFound()
    {
        // Arrange
        var id = 1;
        _mockRepository.GetByIdAsync(id).Returns((BusinessCard)null);

        // Act
        var result = await _service.ExportToCsv(id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(BusinessCardErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task ExportToXml_ShouldReturnSuccess_WhenBusinessCardIsFound()
    {
        // Arrange
        var id = 1;
        var businessCard = new BusinessCard();
        var fileParser = new FileParser();
        var expectedResult = new byte[] { 0x00 };

        _mockRepository.GetByIdAsync(id).Returns(businessCard);
        _mockMapper.Map<FileParser>(businessCard).Returns(fileParser);
        _mockFileParserManager.CreateXMLFile(Arg.Any<List<FileParser>>()).Returns(expectedResult);

        // Act
        var result = await _service.ExportToXml(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult, result.Value);
    }

}
