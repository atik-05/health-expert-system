using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Health_Expert_System.DAL;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using WebMatrix.WebData;

namespace Health_Expert_System.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository = new PatientRepository();

        public void AddPatient(PatientRegistrationViewModel patient)
        {
            WebSecurity.CreateUserAndAccount(patient.UserName, patient.Password);
            WebSecurity.Login(patient.UserName, patient.Password);
            Roles.AddUserToRole(patient.UserName, "Patient");

            var userId = WebSecurity.GetUserId(patient.UserName);
            var patientProfile = new PatientProfile()
            {
                UserId = userId,
                Name = patient.Name,
                Gender = patient.Gender,
                Age = patient.Age,
                Occupation = patient.Occupation,
                MonthlyIncome = patient.MonthlyIncome,
                LifestyleStatus = patient.LifestyleStatus
            };
            
            _patientRepository.AddPatientProfile(patientProfile);
        }

        
    }
}