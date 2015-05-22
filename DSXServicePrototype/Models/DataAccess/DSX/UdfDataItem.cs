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
        [DMLField(TableName.UDF, FieldName = "UdfNum")]
        public int UdfNumber { get; set; }

        [DMLField(TableName.UDF, FieldName = "UdfText")]
        public string UdfText { get; set; }

        /// <summary>
        /// A helper class that represents a single user-defined field
        /// </summary>
        /// <param name="udfNumber">The field number that identifies which user-defined field to apply the updated value.</param>
        /// <param name="udfText">The text value with which to update the user-defined field.</param>
        public UdfDataItem(int udfNumber, string udfText)
        {
            UdfNumber = udfNumber;
            UdfText = udfText;
        }
    }
}
