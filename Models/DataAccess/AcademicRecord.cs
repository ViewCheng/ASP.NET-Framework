using System;
using System.Collections.Generic;

namespace Lab7.Models.DataAccess
{
    public partial class AcademicRecord
    {
        public string CourseCode { get; set; }
        public string StudentId { get; set; }
        public int? Grade { get; set; }

        public Course CourseCodeNavigation { get; set; }
        public Student Student { get; set; }
    }
}
