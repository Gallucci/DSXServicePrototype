using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    /// <summary>
    /// Represents a DSX request to revoke door access from a card holder.
    /// </summary>
    class RevokeAccessRequest : BaseRequest
    {
        // Implementation-specific properties
        
        /// <summary>
        /// The access levels to be removed from the card holder's card.
        /// </summary>
        [DMLField(TableName.Cards, FieldName = "DelAcl")]
        public IList<string> AccessLevels { get; private set; }

        /// <summary>
        /// A Revoke Access request whose content can be used to instruct DSX to revoke access from a card holder's card.
        /// </summary>
        /// <param name="builder">The builder used to construct the Revoke Access request.</param>
        private RevokeAccessRequest(RevokeAccessRequestBuilder builder) : base(builder) 
        {
            AccessLevels = builder.AccessLevels;
        }

        /// <summary>
        /// The builder responsible for constructing a DSX Revoke Access request.
        /// </summary>        
        internal sealed class RevokeAccessRequestBuilder : BaseRequestBuilder
        {
            // Implementation-specific properties            
            public IList<string> AccessLevels { get; private set; }

            /// <summary>
            /// Initializes a builder that helps construct a Revoke Access request for DSX.
            /// </summary>
            /// <param name="firstName">The first name of the card holder.</param>
            /// <param name="lastName">The last name of the card holder.</param>
            /// <param name="company">The company name of the card holder.</param>
            /// <param name="code">The access card number.</param>
            public RevokeAccessRequestBuilder(string firstName, string lastName, string company, long code) 
                : base(firstName, lastName, company, code) 
            {
                Initialize();
            }

            /// <summary>
            /// Initializes a builder that helps construct a Revoke Access request for DSX.
            /// </summary>
            /// <param name="firstName">The first name of the card holder.</param>
            /// <param name="lastName">The last name of the card holder.</param>
            /// <param name="company">The company name of the card holder.</param>
            /// <param name="code">The access card number.</param>
            /// <param name="locGroupNumber">The location group number to which the access card is/will be associated.</param>
            /// <param name="udfFieldNumber">The number of the user-defined field to which the access card is/will be associated.</param>
            /// <param name="udfFieldData">The text value of the user-defined field to which the access card is/will be associated.</param>
            public RevokeAccessRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData) 
                : base(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData) 
            {
                Initialize();
            }

            /// <summary>
            /// Initializes collections and sets default values
            /// </summary>
            private void Initialize()
            {
                AccessLevels = new List<string>();
            }

            /// <summary>
            /// Adds a single access level to be revoked to the access card.
            /// </summary>
            /// <param name="aclName">The name of the access level.</param>
            /// <returns></returns>
            public RevokeAccessRequestBuilder RevokeAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    AccessLevels.Add(aclName);

                return this;
            }

            /// <summary>
            /// Adds a set of access levels to be revoked from the access card.
            /// </summary>
            /// <param name="aclSet">The set of access levels.</param>
            /// <returns></returns>
            public RevokeAccessRequestBuilder RevokeAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    RevokeAccessLevel(aclName);
                }
                return this;
            }

            /// <summary>
            /// Builds the Revoke Access request for DSX.  This operation must be performed in order to produce a useable request.
            /// </summary>
            /// <returns>A Revoke Access request for DSX.</returns>
            public override BaseRequest Build()
            {                
                return new RevokeAccessRequest(this);
            }
        }
    }
}
