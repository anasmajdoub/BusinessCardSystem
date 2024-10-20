using BizCardSystem.Application.BusinessCards.Dtos.Create;
using BizCardSystem.Application.BusinessCards.Dtos.File;
using BizCardSystem.Application.Repositories;
using BizCardSystem.Domain.BusinessCards.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BizCardSystem.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessCardsController(IBusinessCardsService _businessCardsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BusinessCardFilter businessCardFilter)
        {

            var businessCards = await _businessCardsService.GetAllAsync(businessCardFilter);

            return Ok(businessCards);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _businessCardsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IResult> Create(CreateBizRequest createBizRequest, IValidator<CreateBizRequest> validator)
        {
            var result = await validator.ValidateAsync(createBizRequest);

            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            var value = await _businessCardsService.CreateAsync(createBizRequest);
            return Results.Ok(value);
        }

        [HttpPost]
        public async Task<IResult> CreateBusinessCardByFileAsync([FromForm] CreateFromFileRequest file, IValidator<CreateFromFileRequest> validator)
        {
            var result = await validator.ValidateAsync(file);

            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }
            var value = await _businessCardsService.CreateBusinessCardByFileAsync(file.File);


            return Results.Ok(value);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _businessCardsService.DeleteAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToCsv(int id)
        {
            var fileContent = await _businessCardsService.ExportToCsv(id);
            return File(fileContent.Value, "text/csv", "business_card.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ExportToXml(int id)
        {
            var fileContent = await _businessCardsService.ExportToXml(id);
            return File(fileContent.Value, "application/xml", "business_card.xml");
        }
    }
}
