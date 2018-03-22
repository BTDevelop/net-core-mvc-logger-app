﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticLoggerApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeriNumber { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
