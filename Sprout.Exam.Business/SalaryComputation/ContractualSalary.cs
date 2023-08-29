using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Sprout.Exam.Business.SalaryComputation
{
    public class ContractualSalary
    {
        [Display(Name = "Net Income")]
        public double RatePerDay { get; set; }
        [Display(Name = "Worked Days")]
        public double WorkedDays { get; set; }
    }
}
