using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    /// <summary>
    /// The base representation of all requests made for DSX consumption.
    /// </summary>
    abstract class BaseRequest
    {
        /// <summary>
        /// The location group number used to identify a card holder in DSX.
        /// </summary>
        [DMLIdentifier(Component.LocationGroupNumber)]
        public int LocationGroupNumber { get; private set; }

        /// <summary>
        /// The number of the user-defined field used to identify a card holder in DSX.
        /// </summary>
        [DMLIdentifier(Component.UdfFieldNumber)]
        public int UdfFieldNumber { get; private set; }

        /// <summary>
        /// The data contained within the user-defined field used to identify a card holder in DSX.
        /// </summary>
        [DMLIdentifier(Component.UdfFieldData)]
        public string UdfFieldData { get; private set; }

        /// <summary>
        /// The first name of the card holder.
        /// </summary>
        [DMLField(TableName.Names, FieldName = "FName")]
        public string FirstName { get; private set; }

        /// <summary>
        /// The last name of the card holder.
        /// </summary>
        [DMLField(TableName.Names, FieldName = "LName")]
        public string LastName { get; private set; }

        /// <summary>
        /// The company name of the card holder.
        /// </summary>
        [DMLField(TableName.Names, FieldName = "Company")]
        public string Company { get; private set; }

        /// <summary>
        /// The code printed on the front of the card.
        /// </summary>
        [DMLField(TableName.Cards, FieldName = "Code")]
        public long Code { get; private set; }

        /// <summary>
        /// User-defined data to be included with the request
        /// </summary>
        [DMLField]
        public List<UdfDataItem> UdfData { get; private set; }

        /// <summary>
        /// Constructs a request whose content can be used to instruct to DSX to perform a particular set of actions on a card holder's access card
        /// </summary>
        /// <param name="builder">The builder used to construct the request.</param>
        protected BaseRequest(BaseRequestBuilder builder)
        {
            LocationGroupNumber = builder.LocationGroupNumber;
            UdfFieldNumber = builder.UdfFieldNumber;
            UdfFieldData = builder.UdfFieldData;
            FirstName = builder.FirstName;
            LastName = builder.LastName;
            Company = builder.Company;
            Code = builder.Code;
            UdfData = builder.UdfData;
        }
    }

    /// <summary>
    /// A helper class to encapsulate user-defined data
    /// </summary>
    public class UdfDataItem
    {
        /// <summary>
        /// The number associated with the user-defined field
        /// </summary>
        [DMLField(TableName.UDF, FieldName = "UdfNum")]
        public int UdfNumber { get; set; }

        /// <summary>
        /// The data value associated with the user-defined field
        /// </summary>
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
