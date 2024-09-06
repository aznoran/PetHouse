using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Files.Delete;
using PetHouse.Application.Files.Get;
using PetHouse.Application.Files.GetAll;
using PetHouse.Application.Files.Upload;
using PetHouse.Application.Volunteers.AddPet;
using PetHouse.Application.Volunteers.AddPetPhoto;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.Delete;
using PetHouse.Application.Volunteers.UpdateMainInfo;
using PetHouse.Application.Volunteers.UpdateRequisites;
using PetHouse.Application.Volunteers.UpdateSocialNetworks;

namespace PetHouse.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateVolunteerHandler,CreateVolunteerHandler>();
        serviceCollection.AddScoped<IDeleteVolunteerHandler, DeleteVolunteerHandler>();
        serviceCollection.AddScoped<IUpdateVolunteerMainInfoHandler,UpdateVolunteerMainInfoHandler>();
        serviceCollection.AddScoped<IUpdateVolunteerRequisitesHandler,UpdateVolunteerRequisitesHandler>();
        serviceCollection.AddScoped<IUpdateVolunteerSocialNetworksHandler,UpdateVolunteerSocialNetworksHandler>();
        serviceCollection.AddScoped<IAddPetHandler,AddPetHandler>();
        serviceCollection.AddScoped<IAddPetPhotoHandler,AddPetPhotoHandler>();
        
        #region TestingFileProvider
        serviceCollection.AddScoped<FileUploadHandler>();
        serviceCollection.AddScoped<FileDeleteHandler>();
        serviceCollection.AddScoped<FileGetHandler>();
        serviceCollection.AddScoped<FileGetAllHandler>();
        #endregion
        serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return serviceCollection;
    }
}