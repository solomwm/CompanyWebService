﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsoleINNClient.Models
{
    [Serializable]
    public class Company
    {
        public int Id { get; set; }
        public string INN { get; set; }
        public string Name { get; set; }
    }
}