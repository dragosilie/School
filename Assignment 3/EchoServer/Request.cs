//Lukas Christensen, Dragos Ilie & Simon Rove Jensen – Group 1
//Mandatory Exercise 3

using System;
using System.Collections.Generic;
using System.Text;

namespace EchoServer
{
    class Request
    {

        public string Method { get; set; }
        public string Path { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }

        public Request(string request)
        {

        }

    }
}
