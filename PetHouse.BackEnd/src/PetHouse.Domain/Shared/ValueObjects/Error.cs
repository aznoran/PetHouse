using System.Collections;
using PetHouse.Domain.Shared.Enums;

namespace PetHouse.Domain.Shared.ValueObjects;

public record Error
{
    private const string SEPARATOR = "|";

    private Error(
        string code,
        string message,
        ErrorType errorType,
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = errorType;
        InvalidField = invalidField;
    }

    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public string? InvalidField { get; }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new Error(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message) =>
        new Error(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) =>
        new Error(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) =>
        new Error(code, message, ErrorType.Conflict);

    public string Serialize()
    {
        string temp = string.Join(SEPARATOR, Code, Message, Type);

        return temp;
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 3)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        return new Error(parts[0], parts[1], type);
    }
    
    public ErrorList ToErrorList() => new([this]);
}

public class ErrorList : IEnumerable<Error>
{
    private List<Error> _errors;

    public ErrorList(IEnumerable<Error> errors)
    {
        _errors = errors.ToList();
    }

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator ErrorList(List<Error> error)
    {
        return new ErrorList(error);
    }
    
    public static implicit operator ErrorList(Error error)
    {
        return new ErrorList([error]);
    }
}