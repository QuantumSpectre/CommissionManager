﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.GUI.Models
{
    class UserAuthenticationRequest
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}