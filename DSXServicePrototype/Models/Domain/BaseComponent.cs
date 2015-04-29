using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public abstract class BaseComponent
    {
        public string Name { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public BaseDataFormat Format { get; set; }

        public abstract string WriteData();
    }
}
