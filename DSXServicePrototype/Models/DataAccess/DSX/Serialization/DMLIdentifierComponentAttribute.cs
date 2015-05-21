using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{
    public enum Component
    {
        LocationGroupNumber,
        UdfFieldNumber,
        UdfFieldData,        
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class DMLIdentifierComponentAttribute : Attribute
    {
        public Component Name { get; set; }

        public DMLIdentifierComponentAttribute(Component name)
        {
            Name = name;
        }
    }
}
