using Artisan.Tools.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Concept.Tests
{
    public class Course : StorableObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        //public virtual ICollection<Enrollment> Enrollments { get; set; }

        public override string ToString()
        {
            return string.Format("Course: ({0}) {1}", Id, Title);
        }
    }
}
