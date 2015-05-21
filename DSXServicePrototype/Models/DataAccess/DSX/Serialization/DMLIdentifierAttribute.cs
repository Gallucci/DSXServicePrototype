using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAttributes
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
