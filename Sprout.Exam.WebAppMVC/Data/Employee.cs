using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Sprout.Exam.WebAppMVC.Data
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        [Display(Name = "TIN")]
        public string Tin { get; set; }
        [Display(Name = "Type")]
        public int EmployeeTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual EmployeeType EmployeeType { get; set; }
    }
}
