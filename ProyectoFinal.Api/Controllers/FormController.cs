using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models.DBO.DTO;
using ProyectoFinal.Models.Response;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using ProyectoFinal.Models.DBO.Models;
using System.Security.Claims;

namespace ProyectoFinal.Api.Controllers
{
    [Authorize(Policy = "AUTHORIZED")]
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpPost("Get")]
        public async Task<clsResponse<dynamic>> GetForm(FormDto form)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            var rToken = Jwt.validateToken(identity);
            var user = rToken;

            if (user == null)
            {
                return new clsResponse<dynamic> { Error = true, ErrorMessage = "Unauthenticated" };
            }

            if (user.Role.ToUpper() != "USER" && user.Role.ToUpper() != "ADMIN")
            {
                return new clsResponse<dynamic> { Error = true, ErrorMessage = "Unauthorized" };
            }

            return new clsResponse<dynamic> { Error = true, Data = form };
        }

        [HttpPost("Set")]
        public async Task<clsResponse<dynamic>> SetForm(FormDto form)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            var rToken = Jwt.validateToken(identity);
            var user = rToken;

            if (user == null)
            {
                return new clsResponse<dynamic> { Error = true, ErrorMessage = "Unauthenticated" };
            }

            if (user.Role.ToUpper() != "ADMIN")
            {
                return new clsResponse<dynamic> { Error = true, ErrorMessage = "Unauthorized" };
            }

            return new clsResponse<dynamic> { Error = true, Data = form };
        }


    }
}
