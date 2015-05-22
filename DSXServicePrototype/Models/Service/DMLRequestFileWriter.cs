using DSXServicePrototype.Models.DataAccess.DSX;
using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Service
{
    class DMLRequestFileWriter
    {
        public string FileBaseName { get; set; }
        public string FileDirectory { get; set; }
        public string FileExtension { get; set; }
        private bool IsTimeStamped { get; set; }
        private BaseRequest Request { get; set; }

        public DMLRequestFileWriter(BaseRequest request)
        {
            Request = request;
            Initialize();    
        }

        private void Initialize() 
        {
            FileBaseName = Request.GetType().Name;
            FileDirectory = @"C:\ETACC\Requests";
            FileExtension = ".txt";
            IsTimeStamped = true;
        }

        private string GenerateDSXFilename()
        {
            var pattern = "yyyyddMMHHmmss";
            var date = DateTime.Now.ToString(pattern);
            var type = Request.GetType().Name;

            if (IsTimeStamped)
                return string.Format("^imp_{0}_{1}", date, type);
            else
                return string.Format("^imp_{0}", type);
        }

        public void WriteRequest(bool isTimeStamped = true)
        {
            IsTimeStamped = isTimeStamped;

            var filename = GenerateDSXFilename();
            var path = Path.Combine(FileDirectory, filename + FileExtension);            

            // TO DO:  Need to create path if it doesn't exist
            if (!Directory.Exists(FileDirectory))
                Directory.CreateDirectory(FileDirectory);

            // Counter in case of duplicate filenames
            var count = 1;

            //If a file with the same name exists, append a number to the end
            while (File.Exists(path))
            {
                count++;
                var newFilename = string.Format("{0}_{1}", filename, count.ToString());
                path = Path.Combine(FileDirectory, newFilename + FileExtension);
            }

            // Write data
            //var data = Request.Content;
            var data = DMLConvert.SerializeObject(Request);
            //File.WriteAllText(path, data);
        }
    }
}
