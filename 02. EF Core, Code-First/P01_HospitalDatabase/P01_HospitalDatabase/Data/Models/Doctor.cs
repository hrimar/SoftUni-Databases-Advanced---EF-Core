﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class Doctor
    {
        //public Doctor()
        //{
        //    Visitations = new List<Visitation>();
        //}

        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }

        public ICollection<Visitation> Visitations { get; set; } = new List<Visitation>();
    }
}
