using Localiz.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Localiz.Controllers
{
    public class CustomerController : Controller
    {
        private readonly LocalizationService _localizationService;
        public CustomerController(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var name = _localizationService.GetLocalizedString("Name");
            var city = _localizationService.GetLocalizedString("City");
            var position = _localizationService.GetLocalizedString("Position");

            return View();
        }
    }
}
