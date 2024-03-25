using Microsoft.EntityFrameworkCore;
using TestWebApplication.Context;
using TestWebApplication.Models;

namespace TestWebApplication.Services {
    public class DatabaseService(ApplicationDbContext context) {
        private readonly ApplicationDbContext _context = context;

        public async Task SaveConfigurationAsync(ConfigurationNode rootNode) {
            await _context.ConfigurationNodes.AddAsync(rootNode);
            await _context.SaveChangesAsync();
        }

        public async Task<ConfigurationNode> GetConfigurationTreeAsync(int rootId) {
            
            var allNodes = await _context.ConfigurationNodes.ToListAsync();
           
            var nodeDict = allNodes.ToDictionary(n => n.Id);
            foreach (var node in allNodes) {
                if (node.ParentId.HasValue && nodeDict.TryGetValue(node.ParentId.Value, out var parent)) {
                    parent.Children.Add(node);
                }
            }
            return nodeDict[rootId];
        }
    }
}
