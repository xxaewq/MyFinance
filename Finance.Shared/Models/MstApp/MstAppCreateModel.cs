using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Shared.Models.MstApp
{
    public class MstAppCreateModel : MstAppModel
    {
        [JsonPropertyName("created_by")]
        public string? CreateBy { get; set; }
        [JsonPropertyName("create_at")]
        public DateTime? CreateAt { get; set; }
    }
}
