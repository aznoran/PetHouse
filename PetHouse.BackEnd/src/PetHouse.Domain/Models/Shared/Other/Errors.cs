namespace PetHouse.Domain.Shared;

public class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name == null ? " " : $" '{name}' ";
            return Error.Validation("value.is.invalid", $"value {label} is invalid.");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for id '{id}'";
            return Error.Validation("record.not.found", $"record not found{forId}.");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? " " : $" '{name}' ";
            return Error.Validation("length.is.invalid", $"invalid{label}length.");
        }

        public static Error WrongPhoneNumber(string? phoneNumber = null)
        {
            var label = phoneNumber == null ? " " : $" '{phoneNumber}' ";
            return Error.Validation("phone.is.invalid", $"Phone number {label} is invalid.");
        }
    }
}