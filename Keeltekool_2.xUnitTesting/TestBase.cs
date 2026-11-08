using Keeltekool_2.ApplicationServices.Services;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
using Keeltekool_2.xUnitTesting.Macros;
using Keeltekool_2.xUnitTesting.Mock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Web.Mvc;

namespace Keeltekool_2.xUnitTesting
{
    public abstract class TestBase
    {
        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private readonly string _databaseName = Guid.NewGuid().ToString();

        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<ILanguageCoursesServices, LanguageCoursesServices>();
            services.AddScoped<ILecturersServices, LecturersServices>();
            services.AddScoped<IFilesServices, FileServices>();
            services.AddScoped<IStudentsServices, StudentsServices>(); 

            services.AddScoped<IHostEnvironment, MockIHostEnvironment>();

            services.AddDbContext<Keeltekool_2Context>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.ConfigureWarnings(w =>
                    w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }

        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }

        public void Dispose()
        {

        }

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }
    }
}