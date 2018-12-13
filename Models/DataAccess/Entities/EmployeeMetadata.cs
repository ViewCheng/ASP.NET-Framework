using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Lab7.Models.DataAccess;
using Lab7.Models;

namespace Lab7.Models.DataAccess
{
    public class EmployeeMetadata
    {
        [Required(ErrorMessage ="Employee name is required.")]
        [RegularExpression(@"[a-zA-Z]+\s+[a-zA-Z]+",
            ErrorMessage = "Must be in the form of first name followed by last name")]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User name is requited.")]
        [StringLength(30,MinimumLength =3,
            ErrorMessage = "User name lenghth should be more than 3 characters")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "User name lenghth should be more than 5 characters")]
        public string Password { get; set; }
    }
}
