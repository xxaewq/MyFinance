using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction.Entities
{
    public class MstUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Enable { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? CreateBy{ get; set; }
        public DateTime? UpdateAt{ get; set; }
        public string? UpdatedBy { get; set; }
    }
}
