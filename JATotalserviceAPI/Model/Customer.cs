﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class Customer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string houseNumber { get; set; }
        public string road { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public List<Task> tasks { get; set; }
    }
}
