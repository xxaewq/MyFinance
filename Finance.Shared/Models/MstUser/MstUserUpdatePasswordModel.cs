using System.Text.Json.Serialization;

namespace Finance.Shared.Models.MstUser;

public class MstUserUpdatePasswordModel : MstUserModel
{
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
    [JsonPropertyName("update_by")]
    public string? UpdateBy { get; set; }
    [JsonPropertyName("update_at")]
    public DateTime? UpdateAt { get; set; }
}
