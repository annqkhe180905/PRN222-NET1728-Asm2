﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class AccountDTO
    {
        public short AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public int? AccountRole { get; set; }

        public bool? AccountStatus { get; set; }

        public string? AccountPassword { get; set; }
        public string? ImgUrl { get; set; }
        public bool Status { get; set; }
    }
}
