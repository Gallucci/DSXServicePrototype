using DSXServicePrototype.Models.DataAccess.DSX;
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
            var service = new DSXRequestService();            

            var request = new ChangePinRequest.ChangePinRequestBuilder("Test", "Dummy", "MZMO Student", 90202705124)
                .SetPin(4545)
                .Build();

            Console.WriteLine(request.Content);
            Console.WriteLine(service.GetResponse(request).Message);

            request = new GrantAccessRequest.GrantAccessRequestBuilder("Test", "Dummy", "Summer Student", 90202705124)
                .AccessBeginsOn(DateTime.Now)
                .AccessStopsOn(DateTime.Now.AddDays(3))
                .SetPin(4444)
                .GrantAccessLevel("Summer Student")
                .Build();

            Console.WriteLine(request.Content);
            Console.WriteLine(service.GetResponse(request).Message);            

            request = new RevokeAccessRequest.RevokeAccessRequestBuilder("Test", "Dummy", "COCO Student", 90202705124)
                .RevokeAccessLevel(new List<string>() {"COCO Student", "COCO RA" })
                .Build();

            Console.WriteLine(request.Content);
            Console.WriteLine(service.GetResponse(request).Message);            

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); 
        }
    }
}
