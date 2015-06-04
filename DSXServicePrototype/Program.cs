using DSXServicePrototype.Models.DataAccess.DSX;
using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using DSXServicePrototype.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            // DML file writer
            var writer = new DMLRequestFileWriter();
            string output;

            // Change Pin request
            var request = new ChangePinRequest.ChangePinRequestBuilder("Test", "Dummy", "MZMO Student", 90202705124)
                .SetPin(4545)
                .Build();

            output = DMLConvert.SerializeObject(request);
            Console.WriteLine(output);
            writer.WriteRequest(request);

            // Grant Access request
            request = new GrantAccessRequest.GrantAccessRequestBuilder("Test", "Dummy", "MZMO Student", 90202705124)
                .AccessBeginsOn(DateTime.Now)
                .AccessStopsOn(DateTime.Now.AddDays(3))
                .SetPin(4444)
                .GrantAccessLevel("GILA Student")
                .Build();

            output = DMLConvert.SerializeObject(request);
            Console.WriteLine(output);
            writer.WriteRequest(request);

            // Revoke Access request
            request = new RevokeAccessRequest.RevokeAccessRequestBuilder("Test", "Dummy", "GILA Student", 90202705124)
                .RevokeAccessLevel(new List<string>() { "GILA Student", "GILA RA" })
                .Build();

            output = DMLConvert.SerializeObject(request);
            Console.WriteLine(output);
            writer.WriteRequest(request); 

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); 
        }        
    }
}
