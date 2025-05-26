using System.Text.Json.Serialization;

namespace Finance.Shared.Models.MstType
{
    public class MstTypeCreateModel : MstTypeModel
    {
        [JsonPropertyName("created_by")]
        public string? CreateBy { get; set; }
        [JsonPropertyName("create_at")]
        public DateTime? CreateAt { get; set; }
    }
}
