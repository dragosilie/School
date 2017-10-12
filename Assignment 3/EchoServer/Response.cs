//Lukas Christensen, Dragos Ilie & Simon Rove Jensen – Group 1
//Mandatory Exercise 3

using System;
using System.Collections.Generic;
using System.Text;

namespace EchoServer
{
    class Response
    {

        public string Status {get; set; }
        public string Body { get; set; }

        public Response (string status)
        {

            this.Status = status;

        }

        public Response(string status, string body)
        {
            this.Status = status;
            this.Body = body;
        }

        public Response()
        {


        }

    }
}
