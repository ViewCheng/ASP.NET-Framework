using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Lab7.Models.DataAccess;
using Lab7.Models;

namespace Lab7.Models.DataAccess
{
    [ModelMetadataType(typeof(EmployeeMetadata))]

    public partial class Employee
    {

        [NotMapped]
        public List<Role> Roles
        {
            get
            {

                StudentRecordContext context = new StudentRecordContext();

                List<Role> roles = (from r in context.Role
                                    where context.EmployeeRole.Any(er => er.RoleId == r.Id
                                    && er.EmployeeId == this.Id)
                                    select r).ToList<Role>();
                return roles;
            }
        }

    }
}
