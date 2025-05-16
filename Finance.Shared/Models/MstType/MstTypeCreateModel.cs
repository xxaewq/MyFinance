using System.Text.Json.Serialization;

namespace Finance.Shared.Models.MstType
{
    public class MstTypeCreateModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("type_name")]
        public string TypeName { get; set; } = null!;
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
    }
}
