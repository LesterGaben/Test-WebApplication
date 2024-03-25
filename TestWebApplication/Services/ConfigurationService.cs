using TestWebApplication.Models;

namespace TestWebApplication.Services {
    public class ConfigurationService(JSONParsingService jsonParsingService, TextConfigurationParsingService textParsingService, DatabaseService databaseService) : IConfigurationService {
        private readonly JSONParsingService _jsonParsingService = jsonParsingService;
        private readonly TextConfigurationParsingService _textParsingService = textParsingService;
        private readonly DatabaseService _databaseService = databaseService;

        public async Task<(bool IsSuccess, string ErrorMessage, int ConfigurationRootId)> ProcessConfigurationFileAsync(IFormFile configFile) {
            try {
                string content;
                using (var reader = new StreamReader(configFile.OpenReadStream())) {
                    content = await reader.ReadToEndAsync();
                }

                string jsonContent;
                
                if (Path.GetExtension(configFile.FileName).ToLower() == ".txt") {
                    jsonContent = await _textParsingService.ParseTxtToJSONAsync(content);
                }
                else {
                    jsonContent = content;
                }

                var rootNode = await _jsonParsingService.ParseJsonToTreeAsync(jsonContent);
                await _databaseService.SaveConfigurationAsync(rootNode);

                return (true, null, rootNode.Id);
            }
            catch (Exception ex) {
                return (false, $"An error occurred: {ex.Message}", 0);
            }
        }

        public async Task<ConfigurationNode> GetConfigurationTreeAsync(int id) {
            return await _databaseService.GetConfigurationTreeAsync(id);
        }
    }
}
