using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        //public Patient() // - ако не ги инициал. в проп-тата
        //{
        //    Prescriptions = new List<PatientMedicament>();
        //    Visitations = new List<Visitation>();
        //    Diagnoses = new List<Diagnose>();
        //}

        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool HasInsurance { get; set; }

        //TODO:
        public ICollection<Visitation> Visitations { get; set; } = new List<Visitation>();
        public ICollection<Diagnose> Diagnoses { get; set; } = new List<Diagnose>();
        public ICollection<PatientMedicament> Prescriptions { get; set; } = new List<PatientMedicament>();

    }
}
