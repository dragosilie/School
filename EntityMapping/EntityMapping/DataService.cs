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
        //Almost Works
        public Order GetOrder(int id)
        {
            using (var db = new NordWindContext())
            {
               var query =
               (from o in db.Orders
               join od in db.OrderDetails
               on o.Id equals od.OrderId
               join pr in db.Products
               on od.ProductId equals pr.Id
               join c in db.Categories
               on pr.CategoryID equals c.Id
               where o.Id == id
               select new Order
               {
                   Id = o.Id,
                   Date = o.Date,
                 //  Required = o.Required,
                 //  Shipped = o.Shipped,
                   Freight = o.Freight,
                   ShipName = o.ShipName,
                   ShipCity = o.ShipCity,
                   OrderDetails = new List<OrderDetails>
                   {
                       new OrderDetails {
                           UnitPrice = od.UnitPrice,
                           Quantity = od.Quantity,
                           Discount = od.Discount,
                           Product = new Product
                           {
                               Name = pr.Name,
                               Category = new Category
                               {
                                   Name = c.Name
                               }
                           }
                       }
                   }
               }).ToList();

                for (int i = 0; i < query.Count(); i++)
                {
                    Console.WriteLine(query[i].OrderDetails.First().Product.Category.Name);
                }

                return query.First();
            }
        }

        //2
        //Maybe Works
        public List<Order> GetOrderFromShippingName(string shipName)
        {

            using (var db = new NordWindContext())
            {
                var query =
                    from o in db.Orders
                     where o.ShipName.Equals(shipName)
                     select new Order
                     {
                         Id = o.Id,
                         Date = o.Date,
                         ShipName = o.ShipName,
                         ShipCity = o.ShipCity
                     };

                return query.ToList();
            }
        }

        //3
        //Works
        public List<Order> GetOrders()
        {
            using (var db = new NordWindContext())
            {
                var query =
                    from o in db.Orders
                    select new Order{
                         Id = o.Id,
                         Date = o.Date,
                         ShipName = o.ShipName,
                         ShipCity = o.ShipCity };

                return query.ToList();
            }
        }

        //4
        //Works
        public List<OrderDetails> GetOrderDetailsByOrderId(int id)
        {

            using (var db = new NordWindContext())
            {

                var query =
                    (from od in db.OrderDetails
                     join pr in db.Products
                     on od.ProductId equals pr.Id
                     where od.OrderId == id
                     select new OrderDetails {
                         Product = new Product
                         {
                             Name = pr.Name
                         },
                         UnitPrice = od.UnitPrice,
                         Quantity = od.Quantity});

                return query.ToList();

            }
        }


        //5
        //Works
        public List<OrderDetails> GetOrderDetailsByProductId(int id)
        {

            using (var db = new NordWindContext())
            {

                var query =
                    (from od in db.OrderDetails
                     join o in db.Orders
                     on od.OrderId equals o.Id
                     where od.ProductId == id
                     select new OrderDetails{
                         Order = new Order
                         {
                             Date = o.Date
                         },
                         UnitPrice = od.UnitPrice,
                         Quantity = od.Quantity});

                Console.WriteLine(query.First().UnitPrice);

                return query.ToList();

            }
        }

        
        
        
        
        //6
        //Works
        public Product GetProduct(int id)
        {
            using (var db = new NordWindContext())
            {

                var query =
                    from pr in db.Products
                     join c in db.Categories
                     on pr.CategoryID equals c.Id
                     where pr.Id == id
                     select new Product
                     {
                         Name = pr.Name,
                         UnitPrice = pr.UnitPrice,
                         Category = new Category
                         {
                             Name = c.Name
                         }
                     };

                return query.FirstOrDefault();

            }

        }

        //7
        //Works
        public List<Product> GetProductByName(string sub)
        {

            using (var db = new NordWindContext())
            {

                var query = 
                    (from p in db.Products
                    join c in db.Categories
                    on p.CategoryID equals c.Id
                    where p.Name.Contains(sub)
                    select new Product
                    {
                        Name = p.Name,
                        Category = new Category
                        {
                            Name = c.Name
                        }
                    }).OrderBy(x => x.Name).ToList();

                return query.ToList<Product>();

            }

        }

        //8
        //Works
        public List<Product> GetProductByCategory(int id)
        {

            using (var db = new NordWindContext())
            {

                var query =
                    (from c in db.Categories
                     join pr in db.Products
                     on c.Id equals pr.CategoryID
                     join od in db.OrderDetails
                     on pr.Id equals od.ProductId
                     where pr.CategoryID == id
                     select new Product
                     {
                         Name = pr.Name,
                         Category = new Category
                         {
                             Name = c.Name
                         }
                     }).Distinct();

                Console.WriteLine(query.Last().Name);

                return query.ToList();

            }

        }

        //9
        //Works
        public Category GetCategory(int id)
        {

            using (var db = new NordWindContext())
            {

                var query = 
                    (from c in db.Categories
                     where c.Id == id
                     select new Category
                     {
                         Id = c.Id,
                         Name = c.Name,
                         Description = c.Description
                     });

                if(query.FirstOrDefault() != null)
                {

                    Console.WriteLine(query.First());
                    return query.First();
                }
                Console.WriteLine("null");
                return null;

            }
        }







        //10
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

        //11
        public Category CreateCategory(string name, string description)
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

                return category;


            }
        }

        //12
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

        //13
        public bool DeleteCategory(int id)
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
                catch (Exception e)
                {
                    Console.WriteLine("Can't delete");
                    return false;
                }
            }

        }

        


    }
}
