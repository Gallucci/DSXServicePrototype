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
        public string DepositDirectory { get; set; }        

        /// <summary>
        /// Constructs a default DML file writer with default values as described in the application settings.
        /// </summary>
        public DMLRequestFileWriter()
        {
            DepositDirectory = ConfigHelper.GetStringValue("DefaultRequestDepositDirectory");            
        }

        /// <summary>
        /// Constructs a DML file writer that uses the specified directory as the repository location for DML files.
        /// </summary>
        /// <param name="despositDirectory">The directory path to serve as the DML repository location.  If the path does not exist, the writer will attempt to create it.</param>
        public DMLRequestFileWriter(string despositDirectory)
        {
            DepositDirectory = despositDirectory;
        }
        
        /// <summary>
        /// Formats a filename into a proper filename as defined by the DSX file name specification.
        /// </summary>
        /// <param name="baseFileName">Name of the file.</param>
        /// <param name="isTimeStamped">If true, a date in yyyydddMMHHmmss format will be prepended to the file name.</param>
        /// <returns>The formatted file name.</returns>
        private string GenerateDSXFilename(string baseFileName, bool isTimeStamped)
        {
            var dataPattern = "yyyyddMMHHmmss";
            var formattedDate = DateTime.Now.ToString(dataPattern);

            if (isTimeStamped)
                return string.Format("^imp_{0}_{1}", formattedDate, baseFileName);
            else
                return string.Format("^imp_{0}", baseFileName);
        }

        /// <summary>
        /// Writes a DML request file to the location specified by the writter's settings.
        /// </summary>
        /// <param name="obj">The object to write into a DML file.</param>
        /// <param name="customFileName">Overrides the default file name with the specified name.  Default protocol is to use the class name of the object to be written.</param>
        /// <param name="isTimeStamped">If true, the file name will be prepended by a date in yyyydddMMHHmmss format.</param>
        public void WriteRequest(object obj, string customFileName = null, bool isTimeStamped = true)
        {
            // Get the file name
            string fileName;            
            if (string.IsNullOrEmpty(customFileName))
                fileName = obj.GetType().Name;
            else
                fileName = customFileName;

            // Construct the full path
            fileName = GenerateDSXFilename(fileName, isTimeStamped);
            var fileExtension = "txt";
            var path = Path.Combine(DepositDirectory, fileName);
            var fullPath = Path.ChangeExtension(path, fileExtension);

            // If the path doesn exist then create it
            if (!Directory.Exists(DepositDirectory))
                Directory.CreateDirectory(DepositDirectory);

            // Counter in case of duplicate filenames
            var count = 1;

            //If a file with the same name exists, append a number to the end
            while (File.Exists(fullPath))
            {
                count++;
                var newFilename = string.Format("{0}_{1}", fileName, count.ToString());
                path = Path.Combine(DepositDirectory, newFilename);
                fullPath = Path.ChangeExtension(path, fileExtension);
            }

            // Write data            
            var data = DMLConvert.SerializeObject(obj);
            File.WriteAllText(fullPath, data);
        }
    }
}
