using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    class TempStudent
    {
        public TempStudent(string firstName, string lastName, long iso)
        {
            FirstName = firstName;
            LastName = lastName;
            ISO = iso;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long ISO { get; set; }
    }
}
