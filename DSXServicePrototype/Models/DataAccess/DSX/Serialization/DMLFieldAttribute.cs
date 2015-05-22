using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{    
    /// <summary>
    /// Attribute that tags a property as a DML field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class DMLFieldAttribute : Attribute
    {
        /// <summary>
        /// The table under which the property value should be written.
        /// </summary>
        public TableName TableName { get; set; }

        /// <summary>
        /// The field name for the property value as written in the DML.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Indicates the property is a DMLEntry whose value should be inspected, but the value itself should not be mapped
        /// </summary>
        public DMLFieldAttribute() { }

        /// <summary>
        /// Indicates the property is a DMLEntry whose value should be inspected and mapped
        /// </summary>
        /// <param name="name">Specifies the table name in the DML output where the property value should be mapped</param>
        public DMLFieldAttribute(TableName name)
        {
            TableName = name;
        }
    }

    /// <summary>
    /// Possible table name values.
    /// </summary>
    public enum TableName
    {
        Names,
        UDF,
        Images,
        Cards,
    }
}
