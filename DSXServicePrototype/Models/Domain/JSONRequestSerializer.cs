using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public class JSONRequestSerializer : IRequestSerializer
    {
        private DSXRequest request;

        public JSONRequestSerializer(DSXRequest request)
        {
            this.request = request;
        }

        public string Serialize()
        {
            string json = JsonConvert.SerializeObject(request, Formatting.Indented);
            return json;
        }
    }
}
