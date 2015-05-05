using DSXServicePrototype.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Service
{
    class RequestService
    {
        public IRequest GetGrantAccessRequest(string netId, IList<string> accessLevels, DateTime? startDate, DateTime? stopDate)
        {
            //Make some call to a person service

            //Build request
            var udfData = new Dictionary<int, string>()
            {
                {2, "123412341234"}
            };

            var request = new DSXRequest.RequestBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
                .AccessBeginsOn(startDate.Value)
                .AccessStopsOn(stopDate.Value)
                .GrantAccessLevel(accessLevels)
                .SetPin(1234)
                .AddUdfData(udfData)
                .SetMaximumUses(9999)
                .Build();

            return request;
        }

        public void SendRequest(IRequest request)
        {

        }
    }
}
