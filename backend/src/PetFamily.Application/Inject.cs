using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.VolunteersManagement.AddPet;
using PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;
using PetFamily.Application.Features.VolunteersManagement.Create;
using PetFamily.Application.Features.VolunteersManagement.Delete;
using PetFamily.Application.Features.VolunteersManagement.UpdateMainInfo;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<AddPetPhotoHandler>();
        services.AddScoped<AddPetHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}