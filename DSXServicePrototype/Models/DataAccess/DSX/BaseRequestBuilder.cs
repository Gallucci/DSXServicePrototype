using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    /// <summary>
    /// The base request builder for building requests meant for DSX consumption.  Every request should contain a builder to build it.
    /// </summary>
    abstract class BaseRequestBuilder
    {        
        public int LocationGroupNumber { get; private set; }
        public int UdfFieldNumber { get; private set; }
        public string UdfFieldData { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Company { get; private set; }
        public long Code { get; private set; }
        public List<UdfDataItem> UdfData {get; private set;}
        
        /// <summary>
        /// Initializes a builder that helps construct a request for DSX.
        /// </summary>
        /// <param name="firstName">The first name of the card holder.</param>
        /// <param name="lastName">The last name of the card holder.</param>
        /// <param name="company">The company name of the card holder.</param>
        /// <param name="code">The access card number.</param>
        protected BaseRequestBuilder(string firstName, string lastName, string company, long code)
        {            
            LocationGroupNumber = ConfigHelper.GetIntValue("DefaultLocationGroupNumber");
            UdfFieldNumber = ConfigHelper.GetIntValue("DefaultUdfFieldNumber");
            UdfFieldData = code.ToString();
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Code = code;
            UdfData = new List<UdfDataItem>() { new UdfDataItem(UdfFieldNumber, UdfFieldData) };
        }

        /// <summary>
        /// Initializes a builder that helps construct a request for DSX.
        /// </summary>
        /// <param name="firstName">The first name of the card holder.</param>
        /// <param name="lastName">The last name of the card holder.</param>
        /// <param name="company">The company name of the card holder.</param>
        /// <param name="code">The access card number.</param>
        /// <param name="locGroupNumber">The location group number to which the access card is/will be associated.</param>
        /// <param name="udfFieldNumber">The number of the user-defined field to which the access card is/will be associated.</param>
        /// <param name="udfFieldData">The text value of the user-defined field to which the access card is/will be associated.</param>
        protected BaseRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData)
        {
            LocationGroupNumber = locGroupNumber;
            UdfFieldNumber = udfFieldNumber;
            UdfFieldData = udfFieldData;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Code = code;
            UdfData = new List<UdfDataItem>() { new UdfDataItem(UdfFieldNumber, UdfFieldData) };
        }

        /// <summary>
        /// Builds the request for DSX.  This operation must be performed in order to produce a useable request.
        /// </summary>
        /// <returns>A request for DSX.</returns>
        abstract public BaseRequest Build();
    }
}
