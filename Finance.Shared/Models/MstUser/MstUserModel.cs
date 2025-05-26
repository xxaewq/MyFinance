using System.Text.Json.Serialization;

namespace Finance.Shared.Models.MstUser;

public class MstUserModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("user_name")]
    public string UserName { get; set; } = null!;
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = null!;
    //public string PasswordHash { get; set; } = null!;
    //public string PasswordSalt { get; set; } = null!;
    //public bool Enable { get; set; }
}
