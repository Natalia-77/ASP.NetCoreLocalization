using Localiz.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Localiz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //тест
            //додавання сдужб локалізації в контейнер служб.Додавання шляху до ресурсів.
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            //додавання підтримки файлів локалізованих представлень.В цьому прикладі локалізація зразка представлення базується на 
            //суфіксі файла представлення.
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                //додає підтримку локалізованих повідомлень перевірки DataAnnotationLocalization через абстракції IStringLocalizer.
                .AddDataAnnotationsLocalization(options=> {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assembly = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ApplicationResource", assembly.Name);
                    };
                   
                });

            //Встановлюється об"єкт RequestLocalizationOptions,його об"єкту передається список культур та культура по замовчуванню.
            //
            services.Configure<RequestLocalizationOptions>(options =>
            {
                //додавання змінної для підтримуваних культур.Це масив,який містить всі значення культур.
                //Проміжне налаштування локалізації повинно здійснюватись перед іншими проміжними налаштуваннями,які можуть
                //перевіряти мову і регіональні параметри запросу.  
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("uk")

                };
                //SupportedCultures визначає результати функцій,що залежать від мови та регіональних параметрів(форматування дат і часу).
                //SupportedUICultures визначає те,які строчки перекладів (в файлі .resx)шукає об"єкт ResourceManager.
                //ResourceManager шукає пов"язані з мовою і регіональними параметрами строчки,які визначаються значенням CurrentUICulture.  
                options.DefaultRequestCulture = new RequestCulture("uk");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var requestlocalizationOptions = app.ApplicationServices
               .GetService<IOptions<RequestLocalizationOptions>>();

            //ініціалізує об"єкт RequestLocalizationOptions за допомогою ряду значень.А саме, в RequestLocalizationOptions
            //встановлюється поточна культура за допомогою властивості DefaultRequestCulture(59 строчка в цьому класі),
            //а також список підтримуваних культур через властивості SupportedCultures та SupportedUICultures.
            app.UseRequestLocalization(requestlocalizationOptions.Value);

            app.UseStaticFiles();

            app.UseRouting();

           

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Customer}/{action=Create}/{id?}");
            });
        }
    }
}
