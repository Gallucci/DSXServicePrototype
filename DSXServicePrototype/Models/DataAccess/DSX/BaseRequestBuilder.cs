using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX
{
    abstract class BaseRequestBuilder
    {
        public int LocationGroupNumber { get; set; }
        public int UdfFieldNumber { get; set; }
        public string UdfFieldData { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public long Code { get; set; }
        public string RequestContent { get; set; }

        protected BaseRequestBuilder(string firstName, string lastName, string company, long code)
        {            
            LocationGroupNumber = 1;
            UdfFieldNumber = 2;
            UdfFieldData = code.ToString();
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Code = code;
        }

        protected BaseRequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData)
        {
            LocationGroupNumber = locGroupNumber;
            UdfFieldNumber = udfFieldNumber;
            UdfFieldData = udfFieldData;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Code = code;
        }
    }
}
