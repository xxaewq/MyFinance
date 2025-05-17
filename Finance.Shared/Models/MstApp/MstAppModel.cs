using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Shared.Models.MstApp
{
    public class MstAppModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("type_app")]
        public string TypeApp { get; set; } = null!;
        [JsonPropertyName("name_app")]
        public string NameApp { get; set; } = null!;
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
