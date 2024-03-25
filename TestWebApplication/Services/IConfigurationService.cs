using TestWebApplication.Models;

namespace TestWebApplication.Services {
    public interface IConfigurationService {
        Task<(bool IsSuccess, string ErrorMessage, int ConfigurationRootId)> ProcessConfigurationFileAsync(IFormFile configFile);
        Task<ConfigurationNode> GetConfigurationTreeAsync(int id);
    }
}
