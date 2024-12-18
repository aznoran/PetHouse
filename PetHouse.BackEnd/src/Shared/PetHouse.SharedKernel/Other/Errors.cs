﻿using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.SharedKernel.Other;

public static class Errors
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
            return Error.NotFound("record.not.found", $"record not found{forId}.");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? " " : $" '{name}' ";
            return Error.Validation("length.is.invalid", $"invalid{label}length.");
        }
    }

    public static class File
    {
        public static Error FailedToUpload(string? fileName = null)
        {
            var label = fileName == null ? " " : $" '{fileName}' ";
            return Error.Failure("file.upload.error", $"File{label}uploading occured error.");
        }
        public static Error FileNotFound(string? fileName = null)
        {
            var label = fileName == null ? " " : $" '{fileName}' ";
            return Error.NotFound("file.not.found", $"File{label}not found.");
        }
        public static Error BucketNotFound(string? fileName = null)
        {
            var label = fileName == null ? " " : $" '{fileName}' ";
            return Error.NotFound("bucket.not.found", $"Bucket{label}not found.");
        }
        public static Error FailedToDelete(string? fileName = null)
        {
            var label = fileName == null ? " " : $" '{fileName}' ";
            return Error.Failure("file.delete.error", $"File{label}deleting occured error.");
        }
        public static Error FailedToGet(string? fileName = null)
        {
            var label = fileName == null ? " files " : $" '{fileName}' ";
            return Error.Failure("file.get.error", $"{label}getting occured error.");
        }
        public static Error WrongSize(string? fileName = null)
        {
            var label = fileName == null ? " files " : $" '{fileName}' ";
            return Error.Failure("file.size.error", $"{label}wrong size error.");
        }
    }

    public static class Volunteer
    {
        public static Error AlreadyExists(string? label = null, string? fieldName = null)
        {
            var labelT = label == null ? "[label?]" : $"'{label}'";
            var fieldNameT = fieldName == null ? "[fieldName?]" : $"'{fieldName}'";
            return Error.Validation("already.exists",
                $"Volunteer with {fieldNameT} {labelT} already exists.");
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
    
    public static class Discussion
    {
        public static Error LessThanTwoUsers()
        {
            return Error.Validation("invalid.users.count", 
                $"Discussion must be created between two users at least");
        }
        public static Error NotAllowedToDiscussion()
        {
            return Error.Validation("comment.not.allowed", 
                $"User is not a participant of the discussion");
        }
    }
    
    public static class Specie
    {
        public static Error AlreadyExists(string? label = null, string? fieldName = null)
        {
            var labelT = label == null ? "[label?]" : $"'{label}'";
            var fieldNameT = fieldName == null ? "[fieldName?]" : $"'{fieldName}'";
            return Error.Failure("already.exists", $"Specie with {fieldNameT} {labelT} already exists.");
        }
        
        public static Error BreedAlreadyExists(string? label = null, string? fieldName = null)
        {
            var labelT = label == null ? "[label?]" : $"'{label}'";
            var fieldNameT = fieldName == null ? "[fieldName?]" : $"'{fieldName}'";
            return Error.Failure("breed.already.exists", 
                $"Breed with {fieldNameT} {labelT} already exists.");
        }
        
        public static Error SomePetHasThisSpecieOrBreed(Guid? id = null, string? fieldName = null)
        {
            var idT = id == null ? "[label?]" : $"'{id}'";
            var fieldnameT = fieldName == null ? "[fieldName?]" : $"'{fieldName}'";
            return Error.Failure("already.has", 
                $"Pet with {id} already has {fieldnameT} specie or breed.");
        }
    }
    
    public static class Accounts
    {
        public static Error InvalidCredentials()
        {
            return Error.Failure("invalid.credentials", $"Invalid credentials.");
        }
        public static Error InvalidRole()
        {
            return Error.Failure("invalid.role", $"Invalid role.");
        }
    }
    
    public static class Tokens
    {
        public static Error RefreshTokenExpired()
        {
            return Error.Failure("refresh.token.expired", $"Refresh token is expired");
        }
    }
    public static class Database
    {
        public static Error TransactionFailed()
        {
            return Error.Failure("transaction.failed", $"Transaction Failed.");
        }
    }
}