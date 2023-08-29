using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.SalaryComputation
{
    public class RegularSalary
    {
        public int DaysOfWork { get; set; }
        [Display(Name = "Net Income")]
        public double BasicMonthlyRate { get; set; }
        public double Tax { get; set; }
        [Display(Name = "Absent Days")]
        public int AbsentDays { get; set; }
    }
}
