using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{
    public enum TableName
    {     
        Names,        
        UDF,
        Images,
        Cards,
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class DMLFieldAttribute : Attribute
    {
        public TableName TableName { get; set; }
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
}
