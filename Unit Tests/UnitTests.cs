using Microsoft.AspNetCore.Http;
using TestWebApplication.Context;
using TestWebApplication.Services;

namespace Unit_Tests {

    public class ConfigurationServiceTests(ConfigurationServiceFixture fixture) : IClassFixture<ConfigurationServiceFixture> {

        private readonly IConfigurationService _configurationService = fixture.ConfigurationService;
        private readonly ApplicationDbContext _context = fixture.Context;

        [Theory]
        [InlineData("config_simple.json")]
        [InlineData("config_simple.txt")]
        [InlineData("config_medium.json")]
        [InlineData("config_medium.txt")]
        [InlineData("config_complex.json")]
        [InlineData("config_complex.txt")]
        public async Task TestConfigurationProcessing(string fileName) {

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(basePath, "../../../TestFiles", fileName);
            var fileInfo = new FileInfo(filePath);
            using var fileStream = fileInfo.OpenRead();
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "config", fileInfo.Name);

            var (isSuccess, errorMessage, configurationRootId) = await _configurationService.ProcessConfigurationFileAsync(formFile);

            Assert.True(isSuccess);
            Assert.Null(errorMessage);
            Assert.NotEqual(0, configurationRootId);

            var savedConfig = await _context.ConfigurationNodes.FindAsync(configurationRootId);
            Assert.NotNull(savedConfig);
        }
    }
}