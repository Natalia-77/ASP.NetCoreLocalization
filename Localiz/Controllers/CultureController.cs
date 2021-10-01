using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Localiz.Controllers
{
    public class CultureController : Controller
    {
        [HttpPost]

        //в параметри приймає код культури та адресу,куди потрібно поветнутись після встановлення культури.
        public IActionResult SetLanguage(string culture,string returnUrl)
        {
            //додає для кукі з ключем key значення value.
            Response.Cookies.Append(
              CookieRequestCultureProvider.DefaultCookieName,
              CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
              new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return LocalRedirect(returnUrl);
        }
    }
}
