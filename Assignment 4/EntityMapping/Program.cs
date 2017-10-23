﻿using System;
using System.Linq;

namespace EntityMapping
{
    class Program
    {
        static void Main(string[] args)
        {

            DataService d = new DataService();

            /* This is all the test calls of my functions, just ignore them, or use them as you make yours*/

            var order = d.GetOrder(10248);

            Console.WriteLine(order.OrderDetails.Count());

            //var shippingOrder = d.getOrderFromShippingName("The Big Cheese");

            //var orders = d.GetOrders();

            //var orders = d.GetOrderDetailsByOrderId(10248);

            //var orders = d.GetOrderDetailsByProductId(11);

            //var products = d.GetProduct(1);

            //var products = d.GetProductByName("ant");

           // var products = d.GetProductByCategory(10000);

            //Console.WriteLine(products.Count());

            var categorys = d.GetCategory(1);

            Console.WriteLine(categorys.Name);

            //10  var categories = d.GetCategories();

            //11 d.CreateCategory("Test", "Nope");

            //12 d.UpdateCategory(13, "si aici", "muie steaua");

            //13 d.DeleteCategory(15);
            
            using (var db = new NordWindContext())
            {
                
               
                var category = db.Categories.FirstOrDefault(x => x.Id == 11);

                if(category != null)
                {
                    category.Name = "2017 Testing";
                }

                db.SaveChanges();
            }
        }
    }
}
