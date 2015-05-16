using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    class ChangePinRequest : BaseRequest
    {
        private ChangePinRequest(ChangePinRequestBuilder builder) : base(builder) { }

        internal sealed class ChangePinRequestBuilder : BaseRequestBuilder
        {
            // Implementation-specific properties
            private long? Pin { get; set; }

            public ChangePinRequestBuilder(string firstName, string lastName, string company, long code) 
                : base(firstName, lastName, company, code) 
            { 
            
            }

            public ChangePinRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData) 
                : base(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData) 
            { 

            }

            public ChangePinRequestBuilder SetPin(long pin)
            {
                Pin = pin;
                return this;
            }

            public BaseRequest Build()
            {
                // Do converstion into DML here

                return new ChangePinRequest(this);
            }
        }
    }
}
