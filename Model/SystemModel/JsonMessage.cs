using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class JsonMessage
    {
        public bool Success { get; set; }

        public bool success { get { return Success; } }

        public string Message { get; set; }

        public string message { get { return Message; } }

        public string msg { get { return Message; } }

        public JsonMessage(bool success,string message="")
        {
            Success = success;
            Message = message;
        }
    }
}
