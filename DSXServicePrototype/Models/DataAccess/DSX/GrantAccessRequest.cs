using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    class GrantAccessRequest : BaseRequest
    {
        private GrantAccessRequest(GrantAccessRequestBuilder builder) : base(builder) { }

        internal sealed class GrantAccessRequestBuilder : BaseRequestBuilder
        {
            // Implementation-specific properties
            private long? Pin { get; set; }
            private DateTime? StartDate { get; set; }
            private DateTime? StopDate { get; set; }
            private IList<string> AccessLevels { get; set; }

            public GrantAccessRequestBuilder(string firstName, string lastName, string company, long code) 
                : base(firstName, lastName, company, code) 
            {
                Initialize();
            }

            public GrantAccessRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData) 
                : base(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData) 
            {
                Initialize();
            }

            private void Initialize()
            {
                AccessLevels = new List<string>();
            }

            public GrantAccessRequestBuilder SetPin(long pin)
            {
                Pin = pin;
                return this;
            }

            public GrantAccessRequestBuilder AccessBeginsOn(DateTime startDate)
            {
                StartDate = startDate;
                return this;
            }

            public GrantAccessRequestBuilder AccessStopsOn(DateTime stopDate)
            {
                StopDate = stopDate;
                return this;
            }

            public GrantAccessRequestBuilder GrantAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    AccessLevels.Add(aclName);

                return this;
            }

            public GrantAccessRequestBuilder GrantAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    GrantAccessLevel(aclName);
                }
                return this;
            }

            public BaseRequest Build()
            {
                // Do converstion into DML here

                return new GrantAccessRequest(this);
            }
        }
    }
}
