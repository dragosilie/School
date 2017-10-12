//Lukas Christensen, Dragos Ilie & Simon Rove Jensen – Group 1
//Mandatory Exercise 3

using System;
using System.Collections.Generic;
using System.Text;

namespace EchoServer
{
    class Categories
    {
        public int cid { get; set; }
        public string name { get; set; }

        public Categories(int cid, string name)
        {
            this.cid = cid;
            this.name = name;
        }

    }
}
