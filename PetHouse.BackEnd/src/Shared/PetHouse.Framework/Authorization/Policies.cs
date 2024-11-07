namespace PetHouse.Framework.Authorization;

public static class Policies
{
    public static class SpeciesManagement
    {
        public const string Get = "species-management.species.read";
        public const string Create = "species-management.species.create";
        public const string Delete = "species-management.species.delete";
        public const string GetBreed = "species-management.breed.read";
        public const string CreateBreed = "species-management.breed.create";        
        public const string DeleteBreed = "species-management.breed.delete";
    } 
    
    public static class PetManagement
    {
        public const string Get = "pet-management.volunteers.read";
        public const string Create = "pet-management.volunteers.create";
        public const string Update = "pet-management.volunteers.update";
        public const string Delete = "pet-management.volunteers.delete";
        public const string GetPet = "pet-management.pets.read";
        public const string CreatePet = "pet-management.pets.create";
        public const string UpdatePet = "pet-management.pets.update";        
        public const string DeletePet = "pet-management.pets.delete";
    } 
}