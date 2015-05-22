using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    abstract class BaseRequest
    {
        [DMLIdentifierComponent(Component.LocationGroupNumber)]
        public int LocationGroupNumber { get; private set; }

        [DMLIdentifierComponent(Component.UdfFieldNumber)]
        public int UdfFieldNumber { get; private set; }

        [DMLIdentifierComponent(Component.UdfFieldData)]
        public string UdfFieldData { get; private set; }

        [DMLEntry(Section.Names, EntryName = "FName")]
        public string FirstName { get; private set; }

        [DMLEntry(Section.Names, EntryName = "LName")]
        public string LastName { get; private set; }

        [DMLEntry(Section.Names, EntryName = "Company")]
        public string Company { get; private set; }

        [DMLEntry(Section.Cards, EntryName = "Code")]
        public long Code { get; private set; }

        [DMLEntry(Section.UDF)]
        public List<UdfDataItem> UdfData { get; private set; }

        /// <summary>
        /// A request whose content can be used to instruct to DSX to perform a particular set of actions on a card holder's access card
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
}
