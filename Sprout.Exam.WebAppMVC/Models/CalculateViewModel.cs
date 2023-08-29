using Sprout.Exam.Business.SalaryComputation;
using Sprout.Exam.WebAppMVC.Data;

namespace Sprout.Exam.WebAppMVC.Models
{
    public class CalculateViewModel 
    {
        public Employee Employee { get; set; }
        public ContractualSalary Contractual { get; set; }
        public RegularSalary Regular { get; set; }
    }
}
