using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public static class RequestSerializerFactory
    {
        public static IRequestSerializer GetCommandSerializer(DSXRequest command, SerializerFormat format)
        {
            switch(format)
            {
                case SerializerFormat.JSON:
                    return new JSONRequestSerializer(command);
                case SerializerFormat.XML:
                    return new XMLRequestSerializer(command);
            }

            // DML is the default serializer type
            return new DMLRequestSerializer(command);            
        }
    }
}
