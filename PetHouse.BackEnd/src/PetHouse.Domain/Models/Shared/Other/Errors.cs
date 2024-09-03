﻿namespace PetHouse.Domain.Shared;

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
        
    }

    public static class Volunteer
    {
        public static Error AlreadyExists(string? label = null, string? fieldname = null)
        {
            var labelt = label == null ? "[label?]" : $"'{label}'";
            var fieldnamet = fieldname == null ? "[fieldname?]" : $"'{fieldname}'";
            return Error.Validation("already.exists", $"Volunteer with {fieldnamet} {labelt} already exists.");
        }
        public static Error WrongPhoneNumber(string? phoneNumber = null)
        {
            var label = phoneNumber == null ? " " : $" '{phoneNumber}' ";
            return Error.Validation("phone.is.invalid", $"Phone number {label} is invalid.");
        }
        public static Error WrongEmail(string? email = null)
        {
            var label = email == null ? " " : $" '{email}' ";
            return Error.Validation("email.is.invalid", $"Email {label} is invalid.");
        }
    }

}