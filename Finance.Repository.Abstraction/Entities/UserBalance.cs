﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction.Entities
{
    public class UserBalance
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AppId { get; set; }
        public double Balance { get; set; }
        public bool Enable { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string? DeleteBy { get; set; }
    }
}
