using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{    
    /// <summary>
    /// Attribute that tags a property as a DML Identifier.  A DML Identifier must have all three components.  Identifiers always appear at the beginning of a request and are used to ID the card holder.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class DMLIdentifierAttribute : Attribute
    {
        /// <summary>
        /// The name of the component to which the property value will be written.
        /// </summary>
        public Component ComponentName { get; set; }

        /// <summary>
        /// Indicates the property is part of a DSX Identifier and should be written to the indicated component.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        public DMLIdentifierAttribute(Component name)
        {
            ComponentName = name;
        }
    }

    /// <summary>
    /// The possible components for a DML Identifier.
    /// </summary>
    public enum Component
    {
        LocationGroupNumber,
        UdfFieldNumber,
        UdfFieldData,
    }
}
