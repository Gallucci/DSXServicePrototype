using DSXServicePrototype.Models.DataAccess.DSX;
using DSXServicePrototype.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Service
{
    class DSXRequestService
    {
        public ApiResponse GetResponse(BaseRequest request)
        {
            var apiResponse = new ApiResponse();

            apiResponse = SendRequest(request);
            return apiResponse;
        }

        private ApiResponse SendRequest(BaseRequest request)
        {
            try
            {
                var writer = new DMLRequestFileWriter();
                writer.WriteRequest(request);

                // Return response
                var apiResponse = new ApiResponse() 
                { 
                    IsSuccess = true,
                    Message = "Request sent."
                };
                return apiResponse;
            }
            catch (Exception)
            {
                // Log exception

                // Return response
                var apiResponse = new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Request failed."
                };
                return apiResponse;
            }
        }        
    }
}
