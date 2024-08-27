using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensoins;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.ToResponse();
        
        /*if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);*/
    }

}


