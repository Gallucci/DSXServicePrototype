using DSXServicePrototype.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Service
{
    class DSXRequestService
    {
        private string GenerateDSXFilename(string id, bool isTimeStamped = true)
        {
            var pattern = "yyyyddMMHHmmss";
            var date = DateTime.Now.ToString(pattern);

            if (isTimeStamped)
                return string.Format("^imp_{0}_{1}", date, id);
            else
                return string.Format("^imp_{0}", id);
        }

        public DSXRequest GetGrantAccessRequest(string netId, IList<string> accessLevels, DateTime? startDate, DateTime? stopDate, long pin)
        {
            // Make some call to a person service?
            var firstName = "Test";
            var lastName = "Dummy";
            var company = "MZMO Student";
            var code = 90202705124;
            var locGroupNumber = 1;
            var udfFieldNumber = 2;
            var udfFieldData = code.ToString();            
            var udfData = new Dictionary<int, string>() { {udfFieldNumber, udfFieldData} };

            // Build request
            var request = new DSXRequest.RequestBuilder(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData)
                .AccessBeginsOn(startDate.Value)
                .AccessStopsOn(stopDate.Value)
                .GrantAccessLevel(accessLevels)
                .SetPin(pin)
                .AddUdfData(udfData)
                .SetMaximumUses(9999)
                .Build();

            // Return the request
            return request;
        }

        public DSXRequest GetRevokeAccessRequest(string netId, IList<string> accessLevels)
        {
            // Make some call to a person service?
            var firstName = "Test";
            var lastName = "Dummy";
            var company = "MZMO Student";
            var code = 90202705124;
            var locGroupNumber = 1;
            var udfFieldNumber = 2;
            var udfFieldData = code.ToString();
            var udfData = new Dictionary<int, string>() { { udfFieldNumber, udfFieldData } };

            // Build request
            var request = new DSXRequest.RequestBuilder(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData)
                .AddUdfData(udfData)
                .RevokeAccessLevel(accessLevels)
                .Build();

            // Return the request
            return request;
        }

        public DSXRequest GetChangePinRequest(string netId, long pin)
        {
            // Make some call to a person service?
            var firstName = "Test";
            var lastName = "Dummy";
            var company = "MZMO Student";
            var code = 90202705124;
            var locGroupNumber = 1;
            var udfFieldNumber = 2;
            var udfFieldData = code.ToString();
            var udfData = new Dictionary<int, string>() { { udfFieldNumber, udfFieldData } };

            var request = new DSXRequest.RequestBuilder(firstName, lastName, company, code, locGroupNumber, udfFieldNumber, udfFieldData)
                .AddUdfData(udfData)
                .SetPin(pin)
                .Build();

            // Return the request
            return request;
        }

        public void SendRequest(DSXRequest request)
        {
            var id = "ETACC";
            var folder = @"C:\ETACC\Requests";
            var filename = GenerateDSXFilename(id);
            var extension = ".txt";
            var path = Path.Combine(folder, filename + extension);

            // TO DO:  Need to create path if it doesn't exist
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Counter in case of duplicate filenames
            var count = 1;

            //If a file with the same name exists, append a number to the end
            while(File.Exists(path))
            {
                count++;
                var newFilename = string.Format("{0}_{1}", filename, count.ToString());
                path = Path.Combine(folder, newFilename + extension);                
            }

            var serializer = new DMLRequestSerializer(request);
            var data = serializer.Serialize();

            File.WriteAllText(path, data);
        }
    }
}
