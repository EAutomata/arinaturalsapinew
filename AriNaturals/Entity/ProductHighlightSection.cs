using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace AriNaturals.Entity
{
    public class ProductHighlightSection
    {
        public Guid SectionId { get; set; }
        public Guid HighlightId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public string SectionText { get; set; } = string.Empty;
        public string? PointsJson { get; set; }

        [NotMapped]
        public List<string>? SectionPoints
        {
            get => string.IsNullOrEmpty(PointsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(PointsJson);
            set => PointsJson = JsonSerializer.Serialize(value);
        }

        // Navigation
        public ProductHighlight Highlight { get; set; }
    }
}
