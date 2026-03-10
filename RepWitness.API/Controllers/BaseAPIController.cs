// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RepWitness.API.Controllers;

[ApiController]
[Route("api/[controller]/")]
[Produces("application/json")]
// [Authorize(AuthenticationSchemes = "Bearer")]

public class BaseAPIController : ControllerBase
{

}