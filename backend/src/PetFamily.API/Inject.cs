using PetFamily.API.Validation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetFamily.API;

public static class Inject
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSerilog();
        
        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        
        });
        
        return services;
    }
}