using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Shared.Models.MstUser
{
    public class MstUserCreateModel : MstUserModel
    {
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
        //[JsonPropertyName("created_by")]
        //public string? CreatedBy { get; set; }
        //[JsonPropertyName("create_at")]
        //public DateTime? CreateAt { get; set; }
    }
}
