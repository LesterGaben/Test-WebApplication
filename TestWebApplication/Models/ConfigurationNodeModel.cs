using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Models {
    public class ConfigurationNode {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        public string Key { get; set; }

        public string? Value { get; set; }

        public virtual ConfigurationNode Parent { get; set; }

        public virtual ICollection<ConfigurationNode> Children { get; set; } = new HashSet<ConfigurationNode>();
    }
}
