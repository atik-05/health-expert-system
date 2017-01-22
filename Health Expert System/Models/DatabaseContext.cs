using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    public class DatabaseContext: DbContext
    {

        public DatabaseContext() : base("DefaultConnection")
        { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<HospitalProfile> HospitalProfiles { get; set; }
        public DbSet<Specialists> Specialistses { get; set; }
        public DbSet<Symptoms> Symptomses { get; set; }
        public DbSet<DerivedSymptoms> DerivedSymptomses { get; set; }
        public DbSet<SelectionHistory> SelectionHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}