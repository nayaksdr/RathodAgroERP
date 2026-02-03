using Azure;
using JaggeryAgro.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController : ControllerBase
    {
        [HttpPost("set")]
        public IActionResult SetLanguage([FromBody] LanguageRequest request)
        {
            if (string.IsNullOrEmpty(request.Culture))
                return BadRequest("Culture is required");

            var culture = request.Culture;
            var uiCulture = request.UiCulture ?? culture;

            var requestCulture = new RequestCulture(culture, uiCulture);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(requestCulture),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = false
                }
            );

            return Ok(new
            {
                message = "Language changed successfully",
                culture = culture
            });
        }
    }
}
