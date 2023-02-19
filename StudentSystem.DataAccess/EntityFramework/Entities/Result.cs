using System;
using System.Collections.Generic;

namespace StudentSystem.DataAccess.EntityFramework.Entities
{
    public partial class Result
    {
        public string SubjectId { get; set; }
        public string StudentId { get; set; }
        public double? Mark { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
