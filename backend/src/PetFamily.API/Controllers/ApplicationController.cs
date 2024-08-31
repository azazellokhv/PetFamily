using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]

public abstract class ApplicationController : ControllerBase
{
}
