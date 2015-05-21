using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{
    public enum Section
    {     
        Names,
        Cards,
        UDF
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    class DMLEntryAttribute : Attribute
    {
        public Section SectionName { get; set; }
        public string EntryName { get; set; }

        public DMLEntryAttribute(Section name)
        {
            SectionName = name;
        }
    }
}
