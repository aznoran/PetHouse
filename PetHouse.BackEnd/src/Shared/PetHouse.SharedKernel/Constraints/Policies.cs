namespace PetHouse.SharedKernel.Constraints;

public static class Policies
{
    public static class SpeciesManagement
    {
        public const string GetAll = "species-management.species.read";
        public const string Create = "species-management.species.create";
        public const string Delete = "species-management.species.delete";
        public const string GetBreed = "species-management.breed.read";
        public const string CreateBreed = "species-management.breed.create";        
        public const string DeleteBreed = "species-management.breed.delete";
    } 
    
    public static class PetManagement
    {
        public const string GetAll = "pet-management.volunteers.read";
        public const string GetById = "pet-management.volunteers.read";
        public const string GetPets = "pet-management.volunteers.pets.read";
        public const string GetPetById = "pet-management.volunteers.pets.read";
        public const string Create = "pet-management.volunteers.create";
        public const string Delete = "pet-management.volunteers.delete";
        public const string AddPetPhoto = "pet-management.volunteers.pets.pet-photo.create";
        public const string DeletePetPhoto = "pet-management.volunteers.pets.pet-photo.delete";    
        public const string AddPetPhotos = "pet-management.volunteers.pets.pet-photos.create";
        public const string UpdateMainPetPhoto = "pet-management.volunteers.pets.pet-photos.update.main-photo";
        public const string UpdatePetStatus = "pet-management.volunteers.pets.update.pet-status";
        public const string CreatePet = "pet-management.volunteers.pets.create";
        public const string UpdateMainInfo = "pet-management.volunteers.main-info.update";
        public const string UpdateRequisites = "pet-management.volunteers.requisites.update";
        public const string UpdateSocialNetworks = "pet-management.volunteers.social-networks.update";
        public const string UpdatePet = "pet-management.volunteers.pets.update";
        public const string DeletePetForce = "pet-management.volunteers.pets.delete-force";
        public const string DeletePetSoft = "pet-management.volunteers.pets.delete-soft";
    } 
}