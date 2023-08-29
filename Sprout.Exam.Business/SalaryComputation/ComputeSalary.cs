using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.SalaryComputation
{
    public class ComputeSalary
    {
        public EmployeeType employeeType { get; set; }
        public RegularSalary RegularSalary { get; set; }
        public ContractualSalary ContractualSalary { get; set; }

        public ComputeSalary()
        {
            RegularSalary = new RegularSalary();
            ContractualSalary = new ContractualSalary();
        }

        public double ComputationResult()
        {
            double result = 0;

            switch(employeeType)
            {
                case EmployeeType.Regular:
                    var a = RegularSalary.BasicMonthlyRate;
                    var b = RegularSalary.AbsentDays * (RegularSalary.BasicMonthlyRate / RegularSalary.DaysOfWork);
                    var c = RegularSalary.BasicMonthlyRate * RegularSalary.Tax;
                    result = a - b - c;
                    break;
                case EmployeeType.Contractual:
                    result = ContractualSalary.RatePerDay * ContractualSalary.WorkedDays;
                    break;
            }

            return result;
        }
    }
}
