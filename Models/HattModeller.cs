﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hattmakarens_system.Models
{
    public class HattModeller
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public virtual ICollection<Hatt> Hattar { get; set; }
        public virtual ICollection<BildModell> Bilder { get; set; }
        public virtual ICollection<MaterialModell> Material { get; set; }
    }
}