using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Common
{
    public class ServerResponse
    {
        public string Msg { get; set; }
        public Response_Status Status { get; set; }
        public object Data { get; set; }
    }
    public enum Response_Status{
        Success,
        Error
    }
}
