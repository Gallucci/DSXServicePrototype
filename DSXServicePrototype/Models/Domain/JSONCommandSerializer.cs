using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public class JSONCommandSerializer : ICommandSerializer
    {
        private DSXCommand command;

        public JSONCommandSerializer(DSXCommand command)
        {
            this.command = command;
        }

        public string Serialize()
        {
            string json = JsonConvert.SerializeObject(command, Formatting.Indented);
            return json;
        }
    }
}
