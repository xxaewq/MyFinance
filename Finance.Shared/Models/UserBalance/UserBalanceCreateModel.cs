using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Shared.Models.UserBalance
{
    public class UserBalanceCreateModel
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("app_id")]
        public Guid AppId { get; set; }
        [JsonPropertyName("balance")]
        public double Balance { get; set; }
    }
}
