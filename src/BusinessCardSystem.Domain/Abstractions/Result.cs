using System.Diagnostics.CodeAnalysis;

namespace BizCardSystem.Domain.Abstractions;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);



}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => _value!;

    //IsSuccess
    //? _value!
    //: throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}

public sealed class ListResult<TValue> : Result
{
    private readonly IEnumerable<TValue>? _values;
    public int TotalRecord { get; set; }
    public ListResult(IEnumerable<TValue>? values, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _values = values;
    }

    [NotNull]
    public IEnumerable<TValue> Values => IsSuccess
        ? _values!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static ListResult<TValue> From(IEnumerable<TValue>? value) => Create(value);
    public static ListResult<TValue> Success<TValue>(IEnumerable<TValue> value) => new(value, true, Error.None);

    public static ListResult<TValue> Failure<TValue>(IEnumerable<TValue> value, Error error) => new(default, false, error);

    public static ListResult<TValue> Create<TValue>(IEnumerable<TValue>? value) =>
        value is not null && value.Any() ? Success(value) : Failure<TValue>([], Error.NullValue);
}