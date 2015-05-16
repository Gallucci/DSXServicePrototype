using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    class RevokeAccessRequest : BaseRequest
    {
        private RevokeAccessRequest(RevokeAccessRequestBuilder builder) : base(builder) { }

        internal sealed class RevokeAccessRequestBuilder : BaseRequestBuilder
        {
            // Implementation-specific properties
            private IList<string> AccessLevels { get; set; }

            public RevokeAccessRequestBuilder(string firstName, string lastName, string company, long code) 
                : base(firstName, lastName, company, code) 
            {
                Initialize();
            }

            public RevokeAccessRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData) 
                : base(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData) 
            {
                Initialize();
            }

            private void Initialize()
            {
                AccessLevels = new List<string>();
            }

            public RevokeAccessRequestBuilder RevokeAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    AccessLevels.Add(aclName);

                return this;
            }
            public RevokeAccessRequestBuilder RevokeAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    RevokeAccessLevel(aclName);
                }
                return this;
            }
            public BaseRequest Build()
            {
                // Do converstion into DML here

                return new RevokeAccessRequest(this);
            }
        }
    }
}
