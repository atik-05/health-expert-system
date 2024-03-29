﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models.ViewModels
{
    public class ServiceProviderViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Rating { get; set; }

        public int ServiceProviderId { get; set; }
        public int RegistrationNumber { get; set; }
        public int SpecialistId { get; set; }
        public int YearOfExperience { get; set; }
        public int VisitingFee { get; set; }
        public string StartAndEndTime { get; set; }

        public int HospitalTypeId { get; set; }
        public string ListOfSpecialistId { get; set; }
        public int CabinFee { get; set; }
        public bool IsGovt { get; set; }

        public int Fee { get; set; }
        public float Distance { get; set; }

    }
}