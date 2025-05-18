using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Shared.Models.MstApp
{
    public class MstAppUpdateModel : MstAppModel
    {
        [JsonPropertyName("update_by")]
        public string? UpdateBy { get; set; }
        [JsonPropertyName("update_at")]
        public DateTime? UpdateAt { get; set; }
    }
}
