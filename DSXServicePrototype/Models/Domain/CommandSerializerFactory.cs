using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public static class CommandSerializerFactory
    {
        public static ICommandSerializer GetCommandSerializer(DSXCommand command, SerializerFormat format)
        {
            switch(format)
            {
                case SerializerFormat.JSON:
                    return new JSONCommandSerializer(command);
                case SerializerFormat.XML:
                    return new XMLCommandSerializer(command);
            }

            // DML is the default serializer type
            return new DMLCommandSerializer(command);            
        }
    }
}
