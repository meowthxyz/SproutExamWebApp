﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Sprout.Exam.WebAppMVC.Data
{
    public partial class EmployeeType
    {
        public EmployeeType()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
