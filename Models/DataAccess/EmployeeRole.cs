using System;
using System.Collections.Generic;
using Lab7.Models.DataAccess;

namespace Lab7.Models.DataAccess
{
    public partial class EmployeeRole
    {
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }

        public Employee Employee { get; set; }
        public Role Role { get; set; }
    }
}
