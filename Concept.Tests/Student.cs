using Artisan.Tools.Configurator;
using Artisan.Tools.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Concept.Tests
{
    public class Student : StorableObject
    {

        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public List<Enrollment> Enrollments { get; set; }

        public override string ToString()
        {
            return string.Format("({0}) {1}, {2}", Id, LastName, FirstMidName);
        }
    }
}
