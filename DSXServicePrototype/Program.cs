using DSXServicePrototype.Models.Domain;
using DSXServicePrototype.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            //var accessLevels = new List<string>() 
            //{ 
            //    "CORO RA", 
            //    "GILA Student" 
            //};
            //var udfData = new Dictionary<int, string>()
            //{                
            //    {2, "90202705124"}
            //};

            //// Build a Grant Access request
            //var grantAccess = new DSXRequest.RequestBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
            //    .AccessBeginsOn(new DateTime(2015, 1, 15).AddHours(10))
            //    .AccessStopsOn(new DateTime(2015, 1, 18).AddHours(17))                                
            //    .GrantAccessLevel("MZMO Student")
            //    .SetPin(1234)
            //    .AddUdfData(udfData)
            //    .SetMaximumUses(9999)
            //    .Build();

            //// Build a Revoke Access request
            //var revokeAccess = new DSXRequest.RequestBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
            //    .AddUdfData(udfData)
            //    .RevokeAccessLevel("MZMO Student")
            //    .Build();

            //// Build a Change PIN request
            //var changePin = new DSXRequest.RequestBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
            //    .AddUdfData(udfData)
            //    .SetPin(3434)
            //    .Build();

            //// Build a replace code request
            //var replaceCode = new DSXRequest.RequestBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
            //    .AddUdfData(udfData)
            //    .ReplaceCode(123213213132)                
            //    .Build();
            
                        
            //var dmlSerializer = new DMLRequestSerializer(grantAccess);
            //Console.Write(dmlSerializer.Serialize());

            //var jsonSerializer = new JSONRequestSerializer(revokeAccess);
            //Console.Write(jsonSerializer.Serialize());

            //var xmlSerializer = new XMLRequestSerializer(changePin);
            //Console.Write(xmlSerializer.Serialize());

            var drs = new DSXRequestService();
            DSXRequest request;
            
            request = drs.GetGrantAccessRequest("gallucci", new List<string> { "COCO Student" }, DateTime.Now, DateTime.Now.AddDays(3), 4543);
            drs.SendRequest(request);

            request = drs.GetRevokeAccessRequest("gallucci", new List<string> { "COCO Student" });
            drs.SendRequest(request);

            request = drs.GetChangePinRequest("gallucci", 1222);
            drs.SendRequest(request);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); 
        }
    }
}
