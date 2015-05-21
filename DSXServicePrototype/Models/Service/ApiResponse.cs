using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Service
{
    class ApiResponse
    {
        public ApiResponse()
        {

        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
