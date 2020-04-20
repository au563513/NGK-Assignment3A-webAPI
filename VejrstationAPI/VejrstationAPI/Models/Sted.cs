﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VejrstationAPI.Models
{
    public class Sted
    {
        [Key]
        public string Navn { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public List<Vejrobservation> Vejrobservationer { get; set; } //Navigational
    }   
}
