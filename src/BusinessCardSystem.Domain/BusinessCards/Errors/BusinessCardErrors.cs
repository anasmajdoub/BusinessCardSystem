using BizCardSystem.Domain.Abstractions;


namespace BizCardSystem.Domain.BusinessCards.Errors;

public static class BusinessCardErrors
{
    public static readonly Error NotFound = new(
        "BusinessCard.NotFound",
        "The BusinessCard with the specified identifier was not found");
    public static readonly Error Conflict = new(
    "BusinessCard.Conflict",
    "Already added");
}
