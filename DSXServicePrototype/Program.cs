using DSXServicePrototype.Models.DataAccess.DSX;
using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using DSXServicePrototype.Models.Domain;
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
            var service = new DSXRequestService();
            string output;

            var request = new ChangePinRequest.ChangePinRequestBuilder("Test", "Dummy", "MZMO Student", 90202705124)
                .SetPin(4545)
                .Build();

            output = DMLConvert.SerializeObject(request);
            Console.WriteLine(output);
            Console.WriteLine(service.GetResponse(request).Message);

            request = new GrantAccessRequest.GrantAccessRequestBuilder("Test", "Dummy", "Summer Student", 90202705124)
                .AccessBeginsOn(DateTime.Now)
                .AccessStopsOn(DateTime.Now.AddDays(3))
                .SetPin(4444)
                .GrantAccessLevel("Summer Student")
                .Build();

            output = DMLConvert.SerializeObject(request);
            Console.WriteLine(output);
            Console.WriteLine(service.GetResponse(request).Message);

            request = new RevokeAccessRequest.RevokeAccessRequestBuilder("Test", "Dummy", "COCO Student", 90202705124)
                .RevokeAccessLevel(new List<string>() { "COCO Student", "COCO RA" })
                .Build();

            output = DMLConvert.SerializeObject(request);
            Console.WriteLine(output);
            Console.WriteLine(service.GetResponse(request).Message);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); 
        }

        static void GetDMLProperties(BaseRequest request)
        {
            var type = request.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var dmlEntry = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLFieldAttribute));

                if (dmlEntry != null)
                {
                    var toDml = dmlEntry as DMLFieldAttribute;
                    var tableName = toDml.TableName;
                    var fieldName = toDml.FieldName;
                    var entryValue = property.GetValue(request, null);

                    Console.WriteLine(string.Format("The [{0}] class has a DMLEntryAttribute: TableName [{1}], FieldName [{2}], Value [{3}]", type.Name, tableName, fieldName, entryValue));
                }
            }
        }
    }
}
