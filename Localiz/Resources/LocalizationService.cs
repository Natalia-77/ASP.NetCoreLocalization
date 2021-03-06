using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Localiz.Resources
{
    public class LocalizationService
    {
        private readonly IStringLocalizer _localizer;
        public LocalizationService(IStringLocalizerFactory factory)
        {
            var type = typeof(ApplicationResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
            _localizer = factory.Create("ApplicationResource", assemblyName.Name);

        }

        public LocalizedString GetLocalizedString(string key)
        {

            return _localizer[key];
        }
    }
}
