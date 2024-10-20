using AutoMapper;
using BizCardSystem.Application.Abstractions.Data;
using BizCardSystem.Application.BusinessCards.Dtos.Get;
using BizCardSystem.Application.Repositories;
using BizCardSystem.Application.Services;
using BizCardSystem.Domain.BusinessCards;
using BizCardSystem.Domain.BusinessCards.Errors;
using BizCardSystem.Domain.FileHelper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using NSubstitute;
namespace BizCardSystem.Application.UnitTests.BusinessCards;

public class BusinessCardsServiceTests
{
    private readonly IBusinessCardsRepository _mockRepositoryMock;
    private readonly ISqlConnectionFactory _mockSqlConnectionFactory;
    private readonly IMapper _mockMapper;
    private readonly IFileParserManager _mockFileParserManager;
    private readonly IValidator<BusinessCard> _mockValidator;
    private readonly BusinessCardsService _service;

    public BusinessCardsServiceTests()
    {
        _mockRepositoryMock = Substitute.For<IBusinessCardsRepository>();
        _mockSqlConnectionFactory = Substitute.For<ISqlConnectionFactory>();
        _mockMapper = Substitute.For<IMapper>();
        _mockFileParserManager = Substitute.For<IFileParserManager>();
        _mockValidator = Substitute.For<IValidator<BusinessCard>>();

        _service = new BusinessCardsService(
            _mockRepositoryMock,
            _mockSqlConnectionFactory,
            _mockMapper,
            _mockFileParserManager,
            _mockValidator
        );
    }

    [Fact]
    public async Task CreateBusinessCardByFileAsync_ValidFile_CreatesBusinessCards()
    {
        // Arrange
        var mockFile = Substitute.For<IFormFile>();
        var bizResponses = new List<FileParser> { new FileParser(), new FileParser() };
        var businessCards = new List<BusinessCard>
        {
            new BusinessCard { Id = 1 },
            new BusinessCard { Id = 2 }
        };

        _mockFileParserManager.ParseFile(Arg.Any<IFormFile>()).Returns(bizResponses);
        _mockMapper.Map<List<BusinessCard>>(bizResponses).Returns(businessCards);
        _mockValidator.ValidateAsync(Arg.Any<BusinessCard>(), default).Returns(new ValidationResult());

        // Act
        var result = await _service.CreateBusinessCardByFileAsync(mockFile);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(businessCards[0], result.Value);

        await _mockRepositoryMock.Received(2).CreateAsync(Arg.Any<BusinessCard>());
    }

    [Fact]
    public async Task CreateBusinessCardByFileAsync_InvalidBusinessCard_DoesNotCreateInvalidCard()
    {
        // Arrange
        var mockFile = Substitute.For<IFormFile>();
        var bizResponses = new List<FileParser> { new FileParser(), new FileParser() };
        var businessCards = new List<BusinessCard>
            {
                new BusinessCard { Id = 1 },
                new BusinessCard { Id = 2 }
            };

        _mockFileParserManager.ParseFile(Arg.Any<IFormFile>()).Returns(bizResponses);
        _mockMapper.Map<List<BusinessCard>>(bizResponses).Returns(businessCards);

        _mockValidator.ValidateAsync(businessCards[0], default).Returns(new ValidationResult());
        _mockValidator.ValidateAsync(businessCards[1], default).Returns(new ValidationResult
        {
            Errors = { new ValidationFailure("Property", "Error message") }
        });

        // Act
        var result = await _service.CreateBusinessCardByFileAsync(mockFile);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(businessCards[0], result.Value);

        await _mockRepositoryMock.Received(1).CreateAsync(businessCards[0]);
        await _mockRepositoryMock.DidNotReceive().CreateAsync(businessCards[1]);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenBusinessCardExists()
    {
        // Arrange
        int id = 1;
        var businessCard = new BusinessCard { Id = id, Name = "Test" };
        _mockRepositoryMock.GetByIdAsync(id).Returns(businessCard);

        var dto = new GetBizResponse { Id = id, Name = "Test" };
        _mockMapper.Map<GetBizResponse>(businessCard).Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenBusinessCardDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockRepositoryMock.GetByIdAsync(id).Returns((BusinessCard)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(BusinessCardErrors.NotFound, result.Error);
    }
    [Fact]
    public async Task ExportToCsv_ShouldReturnByteArray_WhenBusinessCardExists()
    {
        // Arrange
        int id = 1;
        var businessCard = new BusinessCard { Id = id, Name = "Test" };
        _mockRepositoryMock.GetByIdAsync(id).Returns(businessCard);

        var byteArray = new byte[] { 1, 2, 3 };
        _mockFileParserManager.CreateCSVFile(Arg.Any<List<FileParser>>()).Returns(byteArray);

        // Act
        var result = await _service.ExportToCsv(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(byteArray, result.Value);
    }

    [Fact]
    public async Task ExportToCsv_ShouldReturnFailure_WhenBusinessCardDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockRepositoryMock.GetByIdAsync(id).Returns((BusinessCard)null);

        // Act
        var result = await _service.ExportToCsv(id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(BusinessCardErrors.NotFound, result.Error);
    }

}
