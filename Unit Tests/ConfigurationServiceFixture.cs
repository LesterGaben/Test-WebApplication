using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestWebApplication.Context;
using TestWebApplication.Services;

namespace Unit_Tests {
    public class ConfigurationServiceFixture  {
        public IConfigurationService ConfigurationService { get; private set; }
        public ApplicationDbContext Context { get; private set; }

        public ConfigurationServiceFixture() {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            services.AddTransient<IConfigurationService, ConfigurationService>();
            services.AddTransient<JSONParsingService>();
            services.AddTransient<TextConfigurationParsingService>();
            services.AddTransient<DatabaseService>();

            var serviceProvider = services.BuildServiceProvider();
            ConfigurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            Context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            Context.Database.EnsureCreated();
        }

        public void Dispose() {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

}
