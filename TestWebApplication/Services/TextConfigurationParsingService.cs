using Newtonsoft.Json.Linq;

namespace TestWebApplication.Services {
    public class TextConfigurationParsingService {
        public Task<string> ParseTxtToJSONAsync(string fileContent) {
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rootObject = new JObject();

            foreach (var line in lines) {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(':');
                if (parts.Length < 2) continue;

                JObject currentObject = rootObject;
                for (int i = 0; i < parts.Length - 2; i++) {
                    if (currentObject[parts[i]] == null) {
                        currentObject[parts[i]] = new JObject();
                    }
                    currentObject = (JObject)currentObject[parts[i]];
                }

                currentObject[parts[^2]] = parts[^1];
            }

            return Task.FromResult(rootObject.ToString());
        }

    }
}
