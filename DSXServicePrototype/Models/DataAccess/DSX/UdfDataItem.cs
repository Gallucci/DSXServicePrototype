using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    public class UdfDataItem
    {
        [DMLEntry(Section.UDF, EntryName = "UdfNum")]
        public int UdfNumber { get; set; }

        [DMLEntry(Section.UDF, EntryName = "UdfText")]
        public string UdfText { get; set; }

        public UdfDataItem(int udfNumber, string udfText)
        {
            UdfNumber = udfNumber;
            UdfText = udfText;
        }
    }
}
