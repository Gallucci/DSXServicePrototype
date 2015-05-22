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
    class DMLIdentifierAttribute : Attribute
    {
        public Component ComponentName { get; set; }

        public DMLIdentifierAttribute(Component name)
        {
            ComponentName = name;
        }
    }
}
