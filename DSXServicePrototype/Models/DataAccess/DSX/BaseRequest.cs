using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    abstract class BaseRequest
    {
        /// <summary>
        /// Gets the content of the request.  The content can be used to create a DSX request.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// A request whose content can be used to instruct to DSX to perform a particular set of actions on a card holder's access card
        /// </summary>
        /// <param name="builder">The builder used to construct the request.</param>
        protected BaseRequest(BaseRequestBuilder builder)
        {
            Content = builder.RequestContent;
        }
    }
}
