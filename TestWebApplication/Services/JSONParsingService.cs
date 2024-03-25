using Newtonsoft.Json.Linq;
using TestWebApplication.Models;

namespace TestWebApplication.Services {
    public class JSONParsingService {

        public Task<ConfigurationNode> ParseJsonToTreeAsync(string jsonContent) {
            var jObject = JObject.Parse(jsonContent);
            
            var rootNode = new ConfigurationNode { Key = "root" };
            ParseNode(jObject, rootNode);
            return Task.FromResult(rootNode);
        }

        private void ParseNode(JToken token, ConfigurationNode parentNode) {
            if (token is JObject obj) {
                foreach (var prop in obj.Properties()) {
                    var childNode = new ConfigurationNode { Key = prop.Name, Parent = parentNode };
                    parentNode.Children.Add(childNode);
                    ParseNode(prop.Value, childNode);
                }
            }
            else if (token is JArray array) {
                int index = 0;
                foreach (var item in array) {
                    var childNode = new ConfigurationNode { Key = $"Item{index++}", Parent = parentNode };
                    parentNode.Children.Add(childNode);
                    ParseNode(item, childNode);
                }
            }
            else {
                
                parentNode.Value = token.ToString();
            }
        }
    }
}
