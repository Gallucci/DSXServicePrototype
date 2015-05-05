using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DSXServicePrototype.Models.Domain
{
    class XMLRequestSerializer : IRequestSerializer
    {
        private DSXRequest request;

        public XMLRequestSerializer(DSXRequest request)
        {
            this.request = request;
        }

        public string Serialize()
        {
            return "XML formatting not yet supported";

            //var serializer = new XmlSerializer(command.GetType());
            //using (var writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, command);
            //    return writer.ToString();
            //}
        }
    }
}
