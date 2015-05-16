using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    abstract class BaseRequest
    {
        public string RequestContent { get; set; }

        protected BaseRequest(BaseRequestBuilder builder)
        {
            RequestContent = builder.RequestContent;
        }
    }
}
