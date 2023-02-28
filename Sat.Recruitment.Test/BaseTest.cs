using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sat.Recruitment.Core.Infrastructure;
using Sat.Recruitment.Core.Services;

namespace Sat.Recruitment.Test
{
    public partial class BaseTest
    {
        private static readonly ServiceProvider _serviceProvider;

        static BaseTest()
        {
            var services = new ServiceCollection();

            var rootPath =
                new DirectoryInfo(
                        $"{Directory.GetCurrentDirectory().Split("bin")[0]}{Path.Combine(@"\..\Sat.Recruitment.Api".Split('\\', '/').ToArray())}")
                    .FullName;

            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            webHostEnvironment.Setup(p => p.ContentRootPath).Returns(rootPath);
            webHostEnvironment.Setup(p => p.EnvironmentName).Returns("test");
            webHostEnvironment.Setup(p => p.ApplicationName).Returns("Sat.Recruitment");
            services.AddSingleton(webHostEnvironment.Object);

            //file provider
            services.AddTransient<IFileProvider, FileProvider>();

            //add services
            services.AddTransient<IUserService, UserService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}