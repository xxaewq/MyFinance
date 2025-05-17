using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction.Entities
{
    public class MstApp
    {
        public Guid Id { get; set; }
        public string TypeApp { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool Enable { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
