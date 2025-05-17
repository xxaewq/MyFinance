using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Shared.Models.MstType;

public class MstTypeModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("type_name")]
    public string TypeName { get; set; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
