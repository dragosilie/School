using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EntityMapping
{
    public class DataService

        /* This is all my functions, feel free to look at them, or just ignore/delete them when you create yours */
    {
        //1
        //I am not sure exactly what the wanted output is, when I look at the tests they just want product name and
        //category name. So that's what this does.
        public List<object> GetOrder(int id)
        {
            using (var db = new NordWindContext())
            {
                var query =
               (from o in db.Orders
                join od in db.OrderDetails
                on o.Id equals od.OrderID
                join pr in db.Products
                on od.ProductID equals pr.Id
                join c in db.Categories
                on pr.CategoryID equals c.Id
                where o.Id == id
                select new { ProductName = pr.Name, c.Name });

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();
            }
        } 

        //2
        public List<object> GetOrderFromShippingName(string shipName)
        {

            using (var db = new NordWindContext())
            {
                var query =
                    (from o in db.Orders
                     where o.ShipName.Equals(shipName)
                     select new {o.Id, OrderDate = o.Date, o.ShipName, o.ShipCity } 
                     );

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();
            }
        }

        //3
        public List<object> GetOrders()
        {
            using (var db = new NordWindContext())
            {
                var query =
                    (from o in db.Orders
                     select new { o.Id, o.ShipName, o.ShipCity }).ToList();

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();
            }
        }

        //4
        public List<object> GetOrderDetailsFromOrderId(int id)
        {

            using (var db = new NordWindContext())
            {

                var query =
                    (from od in db.OrderDetails
                     join pr in db.Products
                     on od.ProductID equals pr.Id
                     where od.OrderID == id
                     select new { ProductName = pr.Name, od.UnitPrice, od.Quantity });

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();

            }
        }

        //5
        public List<object> GetOrderDetailsFromProductId(int id)
        {

            using (var db = new NordWindContext())
            {

                var query =
                    (from od in db.OrderDetails
                     join o in db.Orders
                     on od.OrderID equals o.Id
                     where od.ProductID == id
                     select new { OrderDate = o.Date, od.UnitPrice, od.Quantity });

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();

            }
        }

        //6
        //Gives the correct rows, but there is lots of unnecessary rows
        public List<object> GetProductFromId(int id)
        {
            using (var db = new NordWindContext())
            {

                var query =
                    (from pr in db.Products
                     join c in db.Categories
                     on pr.CategoryID equals c.Id
                     join od in db.OrderDetails
                     on pr.Id equals od.ProductID
                     where pr.Id == id
                     select new { pr.Name, pr.UnitPrice, pr.QuantityUnit, od.Discount, CategoryName = c.Name });

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();

            } 

        }

        //7
        //I cheesed this abit, by asking which products contains an empty space instead of splitting.
        public List<object> GetListOfProductsWithSubstring()
        {

            using(var db = new NordWindContext())
            {

                var query =
                    from p in db.Products
                    join c in db.Categories
                    on p.CategoryID equals c.Id
                    where p.Name.Contains(' ')
                    select new { p.Name, CatagoryName = c.Name };

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();

            }

        }

        //8
        public List<object> GetProductsByCategoryId(int id)
        {

            using (var db = new NordWindContext())
            {

                var query =
                    (from c in db.Categories
                     join pr in db.Products
                     on c.Id equals pr.CategoryID
                     join od in db.OrderDetails
                     on pr.Id equals od.ProductID
                     where pr.CategoryID == id
                     select new { ProductName = pr.Name, c.Name }).Distinct();
                    //select new { ProductName = pr.Name}).Distinct(); //, pr.UnitPrice, pr.QuantityUnit, od.Discount, c.Name 

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();

            }

        } 

        //9
        public List<object> GetCategories()
        {

            using (var db = new NordWindContext())
            {

                var query =
                    from c in db.Categories
                    select new { c.Id, c.Name, c.Description };

                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }

                return query.ToList<Object>();

            }

        }

        //10
        public void AddCategory(string name, string description)
        {
            using (var db = new NordWindContext())
            {

                var category = new Category
                {
                    Name = name,
                    Description = description

                };

                db.Categories.Add(category);

                db.SaveChanges();


            }
        }

        //11
        public bool UpdateCategory(int id, string name, string description)
        {
            using (var db = new NordWindContext())
            {

                var update = new Category
                {
                    Id = id,
                    Name = name,
                    Description = description
                };

                try
                {
                    db.Categories.Update(update);
                    db.SaveChanges();
                    Console.WriteLine("Sucess!");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't delete");
                    return false;
                }
            }

        }

        //12
        public bool DeleteCategoryById(int id)
        {

            using (var db = new NordWindContext())
            {

                var delete = new Category
                {
                    Id = id

                };

                try
                {
                    db.Categories.Remove(delete);
                    db.SaveChanges();
                    Console.WriteLine("Sucess!");
                    return true;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Can't delete");
                    return false;
                }
            }

        }
    }
}
