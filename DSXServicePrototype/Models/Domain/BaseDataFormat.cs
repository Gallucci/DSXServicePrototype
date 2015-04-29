using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public abstract class BaseDataFormat
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
