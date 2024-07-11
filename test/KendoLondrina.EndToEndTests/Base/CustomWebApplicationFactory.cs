using System.Linq;
using KenLo.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KenLo.EndToEndTests.Base;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => {
            var options = services.FirstOrDefault(
                x => x.ServiceType == typeof(
                    DbContextOptions<KendoLondrinaDbContext>
                )
            );
            if (options is not null)
                services.Remove(options);
            services.AddDbContext<KendoLondrinaDbContext>(
                options => {
                    options.UseInMemoryDatabase("end2end-test-db");
                }
            );
        });
        base.ConfigureWebHost(builder);
    }

}