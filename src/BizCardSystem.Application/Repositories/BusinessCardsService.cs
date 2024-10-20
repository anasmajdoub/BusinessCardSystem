using AutoMapper;
using BizCardSystem.Application.Abstractions.Data;
using BizCardSystem.Application.BusinessCards.Dtos.Create;
using BizCardSystem.Application.BusinessCards.Dtos.Get;
using BizCardSystem.Application.BusinessCards.Dtos.Update;
using BizCardSystem.Application.Services;
using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Domain.BusinessCards;
using BizCardSystem.Domain.BusinessCards.Errors;
using BizCardSystem.Domain.BusinessCards.Filters;
using BizCardSystem.Domain.FileHelper;
using BizCardSystem.Domain.Shared;
using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Data;
using static Dapper.SqlMapper;

namespace BizCardSystem.Application.Repositories;

public class BusinessCardsService(
    IBusinessCardsRepository _businessCardsRepository,
    ISqlConnectionFactory _sqlConnectionFactory,
    IMapper _mapper,
    IFileParserManager fileParserManager,
    IValidator<BusinessCard> validator
    ) : IBusinessCardsService
{
    public async Task<Result<BusinessCard>> CreateBusinessCardByFileAsync(IFormFile file)
    {
        var bizResponses = fileParserManager.ParseFile(file);
        var businessCards = _mapper.Map<List<BusinessCard>>(bizResponses);

        foreach (var businessCard in businessCards)
        {
            var result = await validator.ValidateAsync(businessCard);

            if (result.IsValid)
            {
                await _businessCardsRepository.CreateAsync(businessCard);
            }
        }

        return Result.Success(businessCards.FirstOrDefault());
    }

    public async Task<Result<int>> CreateAsync(CreateBizRequest entityDto)
    {
        var entity = _mapper.Map<BusinessCard>(entityDto);
        await _businessCardsRepository.CreateAsync(entity);
        return Result.Success<int>(entity.Id);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        await _businessCardsRepository.DeleteAsync(id);
        return Result.Success(id);
    }

    public async Task<ListResult<GetBizResponse>> GetAllAsync(BusinessCardFilter businessCardFilter)
    {
        var filterParameters = _mapper.Map<BusinessCardParameters>(businessCardFilter);
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();


        using var multi = await connection.QueryMultipleAsync(
            "SpGetALLBusinessCards",
            filterParameters,
            commandType: CommandType.StoredProcedure
        );


        var businessCards = multi.Read<BusinessCard, Address, BusinessCard>(
            (businessCard, address) =>
            {
                businessCard.Address = address;
                return businessCard;
            },
            splitOn: "Country"
        );


        var totalRecord = multi.ReadSingle<int>();


        var dto = _mapper.Map<IReadOnlyList<GetBizResponse>>(businessCards.ToList());


        var result = ListResult<GetBizResponse>.Success(dto);
        result.TotalRecord = totalRecord;

        return result;
    }

    public async Task<Result<GetBizResponse>> GetByIdAsync(int id)
    {
        var entity = await _businessCardsRepository.GetByIdAsync(id);
        if (entity is null)
            return Result.Failure<GetBizResponse>(BusinessCardErrors.NotFound);

        var dto = _mapper.Map<GetBizResponse>(entity);
        return Result.Success(dto);
    }

    public async Task<Result<int>> UpdateAsync(UpdateBizRequest entityDto)
    {
        var entity = _mapper.Map<BusinessCard>(entityDto);
        await _businessCardsRepository.UpdateAsync(entity);
        return Result.Success<int>(entity.Id);
    }

    public async Task<Result<byte[]>> ExportToCsv(int id)
    {
        var entity = await _businessCardsRepository.GetByIdAsync(id);

        if (entity is null)
            return Result.Failure<byte[]>(BusinessCardErrors.NotFound);

        var entityDto = _mapper.Map<FileParser>(entity);
        var result = fileParserManager.CreateCSVFile(new List<FileParser> { entityDto });
        if (result is null)
            return Result.Failure<byte[]>(BusinessCardErrors.NotFound);
        return Result.Success(result);
    }

    public async Task<Result<byte[]>> ExportToXml(int id)
    {
        var entity = await _businessCardsRepository.GetByIdAsync(id);
        if (entity is null)
            return Result.Failure<byte[]>(BusinessCardErrors.NotFound);
        var entityDto = _mapper.Map<FileParser>(entity);
        var result = fileParserManager.CreateXMLFile(new List<FileParser> { entityDto });
        if (result is null)
            return Result.Failure<byte[]>(BusinessCardErrors.NotFound);
        return Result.Success(result);
    }


}
public interface IBusinessCardsService
{
    Task<ListResult<GetBizResponse>> GetAllAsync(BusinessCardFilter businessCardFilter);
    Task<Result<GetBizResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateBizRequest entityDto);
    Task<Result<int>> UpdateAsync(UpdateBizRequest entityDto);
    Task<Result> DeleteAsync(int id);
    Task<Result<BusinessCard>> CreateBusinessCardByFileAsync(IFormFile file);
    Task<Result<byte[]>> ExportToCsv(int id);
    Task<Result<byte[]>> ExportToXml(int id);

}