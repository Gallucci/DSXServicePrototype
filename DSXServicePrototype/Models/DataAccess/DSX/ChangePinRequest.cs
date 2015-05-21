using DSXServicePrototype.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    class ChangePinRequest : BaseRequest
    {
        /// <summary>
        /// A Change PIN request whose content can be used to instruct to DSX to change the PIN of a card holder's access card
        /// </summary>
        /// <param name="builder">The builder used to construct the Change PIN request.</param>
        private ChangePinRequest(ChangePinRequestBuilder builder) : base(builder) { }

        internal sealed class ChangePinRequestBuilder : BaseRequestBuilder
        {
            // Implementation-specific properties
            private long? Pin { get; set; }

            /// <summary>
            /// Initializes a builder that helps construct a Change PIN request for DSX.
            /// </summary>
            /// <param name="firstName">The first name of the card holder.</param>
            /// <param name="lastName">The last name of the card holder.</param>
            /// <param name="company">The company name of the card holder.</param>
            /// <param name="code">The access card number.</param>
            public ChangePinRequestBuilder(string firstName, string lastName, string company, long code) 
                : base(firstName, lastName, company, code) 
            {
                Initialize();
            }

            /// <summary>
            /// Initializes a builder that helps construct a Change PIN request for DSX.
            /// </summary>
            /// <param name="firstName">The first name of the card holder.</param>
            /// <param name="lastName">The last name of the card holder.</param>
            /// <param name="company">The company name of the card holder.</param>
            /// <param name="code">The access card number.</param>
            /// <param name="locGroupNumber">The location group number to which the access card is/will be associated.</param>
            /// <param name="udfFieldNumber">The number of the user-defined field to which the access card is/will be associated.</param>
            /// <param name="udfFieldData">The text value of the user-defined field to which the access card is/will be associated.</param>
            public ChangePinRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData) 
                : base(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData) 
            {
                Initialize();
            }

            /// <summary>
            /// Initializes collections and sets default values
            /// </summary>
            private void Initialize()
            {
                Pin = null;
            }

            /// <summary>
            /// Sets the PIN for the access card.
            /// </summary>
            /// <param name="pin">The 4-digit value for the PIN.  The default value is nothing (use current PIN).</param>
            /// <returns></returns>
            public ChangePinRequestBuilder SetPin(long pin)
            {
                Pin = pin;
                return this;
            }

            /// <summary>
            /// Builds the Change Pin request for DSX.  This operation must be performed in order to produce a useable request.
            /// </summary>
            /// <returns>A Change Pin request for DSX.</returns>
            public override BaseRequest Build()
            {
                // Do converstion into DML here
                var dataBuilder = new DMLDataFormat.FormatBuilder(LocationGroupNumber, UdfFieldNumber, UdfFieldData)
                .OpenTable("Names")
                .AddField("FName", FirstName)
                .AddField("LName", LastName)
                .AddField("Company", Company)
                .CloseTableWithWrite()
                .OpenTable("Cards")
                .AddField("Code", Code)
                .AddField("PIN", Pin)
                .CloseTableWithWrite();

                RequestContent = dataBuilder.Output.ToString();

                return new ChangePinRequest(this);
            }
        }
    }
}
