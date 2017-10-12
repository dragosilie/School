//Lukas Christensen, Dragos Ilie & Simon Rove Jensen – Group 1
//Mandatory Exercise 3

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Collections.Generic;

namespace EchoServer
{
    class Program
    {

        //All the possible input Methods.
        public static string[] inputMethods = { "create", "read", "update", "delete", "echo" };

        //List of Categories object. It will be filled with all the "rows".
        public static List<Categories> listOfCategories = new List<Categories>();

        static void Main(string[] args)
        {
            
            //Fill out of list of categories
            addCategories(listOfCategories);

            int port = 5000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            var server = new TcpListener(localAddr, port);

            server.Start();

            Console.WriteLine("Started");

            while (true)
            {
                var client = server.AcceptTcpClient();

                Console.WriteLine("Client connected");

                var thread = new Thread(HandleClient);

                thread.Start(client);
            }

        }

        //Fill out with the given categories
        static void addCategories(List<Categories> categoryList)
        {
            Categories c1 = new Categories(1, "Beverages");
            Categories c2 = new Categories(2, "Condiments");
            Categories c3 = new Categories(3, "Confections");
            categoryList.Add(c1);
            categoryList.Add(c2);
            categoryList.Add(c3);
        }

        //check whether a code number is already in a string
        static Boolean containCode(string code, string message)
        {
            if(message.Contains(code))
            {
                return true;
            }
            return false;
        }

        //add a status message to a string
        static string addStatus(string code, string message, string totalResponse)
        {
            if(containCode(code,totalResponse))
            {
                return totalResponse + message;
            }
            else
            {
                return totalResponse + code + message;
            }
        }

        //Use this method to read and evaluate the stream input
        static Response ReadInput(string s)
        {
            //This string is used to create the whole status response
            string totalResponse = "";

            dynamic inputFromJSON = JsonConvert.DeserializeObject(s);

            //If the input doesn't have a method
            if (inputFromJSON.Method == null)
            {

                string statusCode = "4";
                string statusMessage = " missing method,";

                totalResponse = addStatus(statusCode, statusMessage, totalResponse);
            
            }
            //If the input doesn't have a valid method
            if (!inputMethods.Contains((string)inputFromJSON.Method) && inputFromJSON.Method != null)
            {
                string statusCode = "4";
                string statusMessage = " illegal method,";

                totalResponse = addStatus(statusCode, statusMessage, totalResponse);
            }

            //If method is echo return the body as reponse 
           if(inputFromJSON.Method == "echo" && inputFromJSON.Body != null)
            {
                string statusCode = "1";
                string statusMessage = " Ok";

                string body = (string)inputFromJSON.Body;
                return new Response(addStatus(statusCode, statusMessage, ""),body);    
            } 

            //If the input doesn't have a path and the method isn't "echo"
            if (inputFromJSON.Path == null && inputFromJSON.Method != "echo")
            {

                string statusCode = "4";
                string statusMessage = " missing resource,";

                totalResponse = addStatus(statusCode, statusMessage, totalResponse);
            }

            //If the input have a path
            if (inputFromJSON.Path != null)
            {   
                //Split the path up by using the "/" character - [0]" ", [1] api, [2] categories, [3] index
                string k = (string)inputFromJSON.Path;
                string[] path = k.Split('/');
                string pathCata = "";
                //The index number is -1 as standard, meaning, if the path doesn't have an index the number is -1
                int number = -1;

                //If there is 
                if(path.Length > 2)
                {                   
                    pathCata = path[2];

                }
                //If the path have an index
                if (path.Length > 3)
                {
                    //Parse the string index as a integer (TryParse is used, as a case could be that someone use a string instead of a int (api/categories/one)
                    bool b = Int32.TryParse(path[3], out number);

                    //If the index can't be converted to a integer 
                    if (!b)
                    {
                        number = -2;
                    }
                }

                    //If the path are directed at categories
                    if (pathCata == "categories")
                {
                    //If the method is "create", and a index isn't specified
                    if (inputFromJSON.Method == "create" )
                    {
                        if (number == -1 && inputFromJSON.Body != null) {

                            //Test if I can create a Categories object from a Json input //// This doesn't work! /////
                            //var o = JsonConvert.DeserializeObject<Categories>(inputFromJSON.Body);
                            //listOfCategories.Add(o);

                            string l = (string)JsonConvert.SerializeObject(inputFromJSON.Body);
                            string statusCode = "2";
                            string statusMessage = " Created";

                            return new Response(addStatus(statusCode, statusMessage, ""), l);
                        }  
                        else
                        {
                            string statusCode = "4";
                            string statusMessage = " Bad Request";

                            return new Response(addStatus(statusCode, statusMessage, ""));
                        }
                    }
                    
                    //If the input method is "delete"
                    if(inputFromJSON.Method == "delete")
                    {
                        //As long as they try to delete with an negative index, or with a index too large
                        if(number < listOfCategories.Count && number > 0)
                        {
                            listOfCategories.RemoveAt(number-1);
                            string statusCode = "1";
                            string statusMessage = " Ok";

                            return new Response(addStatus(statusCode, statusMessage, ""));

                        }
                        if(number == -1)
                        {
                            string statusCode = "4";
                            string statusMessage = " Bad Request";

                            return new Response(addStatus(statusCode, statusMessage, ""));
                        }
                        else
                        {
                            string statusCode = "5";
                            string statusMessage = " Not Found";

                            return new Response(addStatus(statusCode, statusMessage, ""));
                        }
                        
                    }
                    //If the input method is "read"
                    if (inputFromJSON.Method == "read")
                    {
                        //If they want to read an object which is in range of the list of objects
                        if (number < listOfCategories.Count && number > 0)
                        {
                            string statusCode = "1";
                            string statusMessage = " Ok";

                            Categories body = listOfCategories.ElementAt(number - 1);

                            var convertedBody = JsonConvert.SerializeObject(body);

                            return new Response(addStatus(statusCode, statusMessage, ""), convertedBody);
                        }
                        //If they want to read all the objects
                        if(number == -1)
                        {
                            string statusCode = "1";
                            string statusMessage = " Ok";

                            var convertedBody = JsonConvert.SerializeObject(listOfCategories);

                            return new Response(addStatus(statusCode, statusMessage, ""), convertedBody);
                        }
                        //If they try to read an object where the index is too large
                        if(number > listOfCategories.Count)
                        {
                            string statusCode = "5";
                            string statusMessage = " Not Found";

                            return new Response(addStatus(statusCode, statusMessage, ""));
                        }
                        //If they give a faulty index (like "xxx")
                        else
                        {
                            string statusCode = "4";
                            string statusMessage = " Bad Request";

                            return new Response(addStatus(statusCode, statusMessage, ""));
                        }
                    }
                }
                    else
                {
                    //
                    if(inputFromJSON.Method == "read")
                    {
                        string statusCode = "4";
                        string statusMessage = " bad request";


                        return new Response(addStatus(statusCode, statusMessage, ""));
                    }
                }
            } 

            //If the input doesn't have a date
            if (inputFromJSON.Date == null)
            {
                string statusCode = "4";
                string statusMessage = " missing date,";

                totalResponse = addStatus(statusCode, statusMessage, totalResponse);
            }
            //If the input have a date
            else
            {
                string dateString = (string)inputFromJSON.Date;
                //Check if it is in Unix        /// We couldn't find a method doing this, so we did this instead ///
                if (dateString.Contains(":") || dateString.Contains(" ") || dateString.Contains("/"))
                {
                    string statusCode = "4";
                    string statusMessage = " illegal date,";

                    totalResponse = addStatus(statusCode, statusMessage, totalResponse);
                }
            }
            
            //If the input body is empty
            if(inputFromJSON.Body == null)
            {
                string statusCode = "4";
                string statusMessage = " missing body,";

                totalResponse = addStatus(statusCode, statusMessage, totalResponse);
            }
            //If the body isn't empty
           else
            {

                string m = (string) inputFromJSON.Body;

                var bodyArray = m.ToCharArray();

                Console.WriteLine("Test");

                //Check if it is a Json
                if (bodyArray[0] != '{' || bodyArray[0] != '[' )
                {
                    string statusCode = "4";
                    string statusMessage = " illegal body,";

                    totalResponse = addStatus(statusCode, statusMessage, totalResponse);
                }
            }

            //Console.WriteLine(totalResponse);         -Test print of the result

            return new Response(totalResponse);
        }

        static void HandleClient(object clientObj)
        {

            var client = clientObj as TcpClient;
            if (client == null) return;

            var strm = client.GetStream();

            try
            {
                var request = Read(strm, client.ReceiveBufferSize);

                var convertedRequest = JsonConvert.SerializeObject(request);

                var serverResponse = ReadInput(convertedRequest);
                Console.WriteLine(serverResponse);
            
                Send(strm, JsonConvert.SerializeObject(serverResponse));

                strm.Close();
                client.Dispose();
            }
            catch (IOException)
            {
                Console.WriteLine("No request!!!");
            }

            
        }


        static void Send(NetworkStream strm, string data)
        {
            var response = Encoding.UTF8.GetBytes(data);
            Console.WriteLine($"Response: {data}");
            strm.Write(response, 0, response.Length);
        }

        static Request Read(NetworkStream strm, int size)
        {
            byte[] buffer = new byte[size];
            var bytesRead = strm.Read(buffer, 0, buffer.Length);
            var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Request: {JsonConvert.SerializeObject(request)}");
            return JsonConvert.DeserializeObject<Request>(request);
        }

    }
}
