using DSXServicePrototype.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    class GrantAccessRequest : BaseRequest
    {
        /// <summary>
        /// A Grant Accesss request whose content can be used to instruct to DSX to grant access levels to a card holder's access card
        /// </summary>
        /// <param name="builder">The builder used to construct the Grant Access request.</param>
        private GrantAccessRequest(GrantAccessRequestBuilder builder) : base(builder) { }

        internal sealed class GrantAccessRequestBuilder : BaseRequestBuilder
        {
            // Implementation-specific properties
            private long? Pin { get; set; }
            private long? NumberOfUses { get; set; }
            private DateTime? StartDate { get; set; }
            private DateTime? StopDate { get; set; }
            private IList<string> AccessLevels { get; set; }

            /// <summary>
            /// Initializes a builder that helps construct a Grant Access request for DSX.
            /// </summary>
            /// <param name="firstName">The first name of the card holder.</param>
            /// <param name="lastName">The last name of the card holder.</param>
            /// <param name="company">The company name of the card holder.</param>
            /// <param name="code">The access card number.</param>
            public GrantAccessRequestBuilder(string firstName, string lastName, string company, long code) 
                : base(firstName, lastName, company, code) 
            {
                Initialize();
            }

            /// <summary>
            /// Initializes a builder that helps construct a Grant Access request for DSX.
            /// </summary>
            /// <param name="firstName">The first name of the card holder.</param>
            /// <param name="lastName">The last name of the card holder.</param>
            /// <param name="company">The company name of the card holder.</param>
            /// <param name="code">The access card number.</param>
            /// <param name="locGroupNumber">The location group number to which the access card is/will be associated.</param>
            /// <param name="udfFieldNumber">The number of the user-defined field to which the access card is/will be associated.</param>
            /// <param name="udfFieldData">The text value of the user-defined field to which the access card is/will be associated.</param>
            public GrantAccessRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData) 
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
                NumberOfUses = 9999;
                StartDate = DateTime.Now;
                StopDate = null;
                AccessLevels = new List<string>();                
            }

            /// <summary>
            /// Sets the PIN for the access card.
            /// </summary>
            /// <param name="pin">The 4-digit value for the PIN.  The default value is nothing (use current PIN).</param>
            /// <returns></returns>
            public GrantAccessRequestBuilder SetPin(long pin)
            {
                Pin = pin;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of uses the access card can be used.
            /// </summary>
            /// <param name="number">The maximum number of uses the access card can be used.  The default value is 9999 (unlimited).</param>
            /// <returns></returns>
            public GrantAccessRequestBuilder SetMaximumUses(int number)
            {
                NumberOfUses = number;
                return this;
            }

            /// <summary>
            /// Sets the date and time on which the granted access levels will become active for the access card.
            /// </summary>
            /// <param name="startDate">The starting date and time.  The default value is the current date and time.</param>
            /// <returns></returns>
            public GrantAccessRequestBuilder AccessBeginsOn(DateTime startDate)
            {
                StartDate = startDate;
                return this;
            }

            /// <summary>
            /// Sets the date and time on which the granted access levels will become inactive for the access card.
            /// </summary>
            /// <param name="stopDate">The ending date and time.  The default value is nothing (endless).</param>
            /// <returns></returns>
            public GrantAccessRequestBuilder AccessStopsOn(DateTime stopDate)
            {
                StopDate = stopDate;
                return this;
            }

            /// <summary>
            /// Adds a single access level to be granted to the access card.
            /// </summary>
            /// <param name="aclName">The name of the access level.</param>
            /// <returns></returns>
            public GrantAccessRequestBuilder GrantAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    AccessLevels.Add(aclName);

                return this;
            }

            /// <summary>
            /// Adds a set of access levels to be granted to the access card.
            /// </summary>
            /// <param name="aclSet">The set of access levels.</param>
            /// <returns></returns>
            public GrantAccessRequestBuilder GrantAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    GrantAccessLevel(aclName);
                }
                return this;
            }

            /// <summary>
            /// Builds the Grant Access request for DSX.  This operation must be performed in order to produce a useable request.
            /// </summary>
            /// <returns>A Grant Access request for DSX.</returns>
            public override BaseRequest Build()
            {
                var aclLevels = new List<Tuple<string, string>>();
                foreach (var level in AccessLevels)
                {
                    aclLevels.Add(new Tuple<string, string>("AddAcl", level));
                }

                // Do converstion into DML here                
                var dataBuilder = new DMLDataFormat.FormatBuilder(LocationGroupNumber, UdfFieldNumber, UdfFieldData)
                .OpenTable("Names")
                .AddField("FName", FirstName)
                .AddField("LName", LastName)
                .AddField("Company", Company)
                .CloseTableWithWrite()
                .OpenTable("UDF")
                .AddField("UdfNum", UdfFieldNumber)
                .AddField("UdfText", UdfFieldData)
                .CloseTableWithWrite()
                .OpenTable("Cards")
                .AddField("Code", Code)
                .AddField("PIN", Pin)                
                .AddField("StartDate", StartDate)
                .AddField("StopDate", StopDate)
                .AddField("NumUses", NumberOfUses)
                .AddField(aclLevels)
                .CloseTableWithWrite();

                RequestContent = dataBuilder.Output.ToString();

                return new GrantAccessRequest(this);
            }
        }
    }
}
