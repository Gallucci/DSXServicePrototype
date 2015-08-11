using DSXServicePrototype.Models.DataAccess.DSX;
using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using DSXServicePrototype.Models.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
            //// DML file writer
            //var writer = new DMLRequestFileWriter();
            //string output;

            //// Change Pin request
            //var request = new ChangePinRequest.ChangePinRequestBuilder("Test", "Dummy", "MZMO Student", 90202705124)
            //    .SetPin(4545)
            //    .Build();

            //output = DMLConvert.SerializeObject(request);
            //Console.WriteLine(output);
            //writer.WriteRequest(request);

            //// Grant Access request
            //request = new GrantAccessRequest.GrantAccessRequestBuilder("Test", "Dummy", "MZMO Student", 90202705124)
            //    .AccessBeginsOn(DateTime.Now)
            //    .AccessStopsOn(DateTime.Now.AddDays(3))
            //    .SetPin(4444)
            //    .GrantAccessLevel("GILA Student")
            //    .Build();

            //output = DMLConvert.SerializeObject(request);
            //Console.WriteLine(output);
            //writer.WriteRequest(request);

            //// Revoke Access request
            //request = new RevokeAccessRequest.RevokeAccessRequestBuilder("Test", "Dummy", "GILA Student", 90202705124)
            //    .RevokeAccessLevel(new List<string>() { "GILA Student", "GILA RA" })
            //    .Build();

            //output = DMLConvert.SerializeObject(request);
            //Console.WriteLine(output);
            //writer.WriteRequest(request); 

            //--CONTINGENCY--

            int counter = 0;
            int numEntries = 1;
            int numBadEntries = 0;
            string line;
            BaseRequest request;

            // Set up manual import directory
            var importPath = ConfigHelper.GetStringValue("DefaultManualImportDirectory");
            var fileName = ConfigHelper.GetStringValue("DefaultManualImportFileName");
            var fullPAth = Path.Combine(importPath, fileName);
            var file = new StreamReader(@fullPAth);          

            while ((line = file.ReadLine()) != null)
            {
                // Split file line into individual values
                var values = line.Split(',');
                
                // First line is headers.  Don't process it.
                if (counter > 0)
                {
                    var lastName = values[0];
                    var firstName = values[1];
                    var iso = values[2];
                    var accessStartDate = values[3];
                    var accessEndDate = values[4];
                    var accessPlan = string.Format("{0} {1}", values[5], values[6]);
                    var pin = values[7];
 
                    var output = string.Format("({0}): {1}, {2}, {3}, {4}, {5}, {6}, {7}", numEntries, lastName, firstName, iso, accessStartDate, accessEndDate, accessPlan, pin);
                    
                    long parsedIso = 0;                    
                    long parsedPin = 0;
                    DateTime parsedAccessStartDate = DateTime.Now;
                    DateTime parsedAccessEndDate = DateTime.Now;

                    // Check for valid values
                    var validPin = !string.IsNullOrEmpty(pin) && pin.Length == 4 && long.TryParse(pin, out parsedPin);
                    var validIso = !string.IsNullOrEmpty(iso) && long.TryParse(iso, out parsedIso);
                    var validAccessStartDate = !string.IsNullOrEmpty(accessStartDate) && DateTime.TryParse(accessStartDate, out parsedAccessStartDate);
                    var validAccessEndDate = !string.IsNullOrEmpty(accessEndDate) && DateTime.TryParse(accessEndDate, out parsedAccessEndDate);

                    // Only generate if there's a valid ISO
                    if (validIso)
                    {                        
                        // Default access start date is now
                        if (!validAccessStartDate)
                            parsedAccessStartDate = DateTime.Now;

                        // Default access end date is 1/1/2016
                        if(!validAccessEndDate)
                            parsedAccessEndDate = new DateTime(2016, 1, 1);

                        // Requst with PIN
                        if (validPin)
                        {
                            request = new GrantAccessRequest.GrantAccessRequestBuilder(firstName, lastName, accessPlan, parsedIso)
                                .AccessBeginsOn(parsedAccessStartDate)
                                .AccessStopsOn(parsedAccessEndDate)
                                .GrantAccessLevel(accessPlan)
                                .SetPin(parsedPin)
                                .Build();
                        }
                        else // Request without PIN
                        {
                            request = new GrantAccessRequest.GrantAccessRequestBuilder(firstName, lastName, accessPlan, parsedIso)
                                .AccessBeginsOn(parsedAccessStartDate)                                
                                .AccessStopsOn(parsedAccessEndDate)
                                .GrantAccessLevel(accessPlan)
                                .Build();
                        }

                        // Serialize and write out the request
                        var writer = new DMLRequestFileWriter();                        
                        DMLConvert.SerializeObject(request);
                        writer.WriteRequest(request);

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(output);
                        Console.ResetColor();                        
                    }
                    else // Bad or missing ISO
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(output);
                        Console.ResetColor();

                        // Increment the number of bad entries
                        numBadEntries++;
                    }

                    // Increment the number of entries
                    numEntries++;
                }

                // Increment the file line counter
                counter++;                
            }

            // Close the file
            file.Close();

            // Write number of entries/bad entries
            Console.WriteLine();
            Console.WriteLine("{0} entries total.  {1} entries with a blank or invalid ISO.", (numEntries -1), numBadEntries);
            Console.ReadKey();

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey(); 
        }        
    }
}
