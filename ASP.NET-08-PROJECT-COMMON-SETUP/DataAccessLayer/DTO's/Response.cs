using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO_s
{
    public class Response
    {
        public int statusCode { get; set; }

        public string message { get; set; }
    }

    public class ServiceResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }  
    }
}
