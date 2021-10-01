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
            //��������� ����� ���������� � ��������� �����.��������� ����� �� �������.
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            //��������� �������� ����� ������������ ������������.� ����� ������� ���������� ������ ������������� �������� �� 
            //������ ����� �������������.
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                //���� �������� ������������ ���������� �������� DataAnnotationLocalization ����� ���������� IStringLocalizer.
                .AddDataAnnotationsLocalization(options=> {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assembly = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ApplicationResource", assembly.Name);
                    };
                   
                });

            //�������������� ��"��� RequestLocalizationOptions,���� ��"���� ���������� ������ ������� �� �������� �� ������������.
            //
            services.Configure<RequestLocalizationOptions>(options =>
            {
                //��������� ����� ��� ������������ �������.�� �����,���� ������ �� �������� �������.
                //������� ������������ ���������� ������� ������������ ����� ������ ��������� ��������������,�� ������
                //��������� ���� � ��������� ��������� �������.  
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("uk")

                };
                //SupportedCultures ������� ���������� �������,�� �������� �� ���� �� ����������� ���������(������������ ��� � ����).
                //SupportedUICultures ������� ��,�� ������� ��������� (� ���� .resx)���� ��"��� ResourceManager.
                //ResourceManager ���� ���"���� � ����� � ������������ ����������� �������,�� ������������ ��������� CurrentUICulture.  
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

            //�������� ��"��� RequestLocalizationOptions �� ��������� ���� �������.� ����, � RequestLocalizationOptions
            //�������������� ������� �������� �� ��������� ���������� DefaultRequestCulture(59 ������� � ����� ����),
            //� ����� ������ ������������ ������� ����� ���������� SupportedCultures �� SupportedUICultures.
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
