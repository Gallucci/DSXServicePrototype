using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    abstract class BaseRequestBuilder
    {
        [DMLIdentifierComponent(Component.LocationGroupNumber)]
        protected int LocationGroupNumber { get; set; }

        [DMLIdentifierComponent(Component.UdfFieldNumber)]
        protected int UdfFieldNumber { get; set; }

        [DMLIdentifierComponent(Component.UdfFieldData)]
        protected string UdfFieldData { get; set; }

        [DMLEntry(Section.Names, EntryName="FName")]
        protected string FirstName { get; set; }

        [DMLEntry(Section.Names, EntryName = "LName")]
        protected string LastName { get; set; }

        [DMLEntry(Section.Names, EntryName = "Company")]
        protected string Company { get; set; }

        [DMLEntry(Section.Cards, EntryName = "Code")]
        protected long Code { get; set; }

        public string RequestContent { get; protected set; }

        /// <summary>
        /// Initializes a builder that helps construct a request for DSX.
        /// </summary>
        /// <param name="firstName">The first name of the card holder.</param>
        /// <param name="lastName">The last name of the card holder.</param>
        /// <param name="company">The company name of the card holder.</param>
        /// <param name="code">The access card number.</param>
        protected BaseRequestBuilder(string firstName, string lastName, string company, long code)
        {            
            LocationGroupNumber = 1;
            UdfFieldNumber = 2;
            UdfFieldData = code.ToString();
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Code = code;
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
        }

        /// <summary>
        /// Builds the request for DSX.  This operation must be performed in order to produce a useable request.
        /// </summary>
        /// <returns>A request for DSX.</returns>
        abstract public BaseRequest Build();
    }
}
