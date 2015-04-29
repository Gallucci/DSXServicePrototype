using DSXServicePrototype.Models.Domain;
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
            var accessLevels = new List<string>() 
            { 
                "CORO RA", 
                "GILA Student" 
            };
            var udfData = new Dictionary<int, string>()
            {                
                {2, "90202705124"}
            };

            // Build a Grant Access command
            var grantAccessCmd = new DSXCommand.CommandBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
                .AccessBeginsOn(new DateTime(2015, 1, 15).AddHours(10))
                .AccessStopsOn(new DateTime(2015, 1, 18).AddHours(17))                                
                .GrantAccessLevel("MZMO Student")
                .SetPin(1234)
                .AddUdfData(udfData)
                .SetMaximumUses(9999)
                .Build();

            // Build a Revoke Access command
            var revokeAccessCmd = new DSXCommand.CommandBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
                .AddUdfData(udfData)
                .RevokeAccessLevel("MZMO Student")                                
                .Build();

            // Build a Change PIN command
            var changePinCmd = new DSXCommand.CommandBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
                .AddUdfData(udfData)
                .SetPin(3434)                
                .Build();

            // Build a replace code command
            var replaceCodeCmd = new DSXCommand.CommandBuilder("Test", "Dummy", "MZMO Student", 90202705124, 1, 2, "90202705124")
                .AddUdfData(udfData)
                .ReplaceCode(123213213132)                
                .Build();
            
            //Console.Write(grantAccessCmd.WriteCommand());
            //Console.Write(revokeAccessCmd.WriteCommand());
            Console.Write(changePinCmd.WriteCommand());
            //Console.Write(replaceCodeCmd.WriteCommand());

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); 
        }
    }
}
