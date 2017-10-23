using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EntityMapping
{
    public class DataService : IDataService

    {
        

        //1
        public Order GetOrder(int id)
        {
            using (var db = new NordWindContext())
            {
                var get_order_q =

                 (from o in db.Orders
                  join od in db.OrderDetails on o.Id equals od.OrderId
                  join pr in db.Products on od.ProductId equals pr.Id
                  join c in db.Categories on pr.CategoryID equals c.Id
                  where o.Id == id
                  select new Order
                  {
                      Id = o.Id,
                      Date = o.Date,
                  //    RequiredDate = o.RequiredDate,
                  //    ShippedDate = o.ShippedDate,
                      Freight = o.Freight,
                      ShipName = o.ShipName,
                      ShipCity = o.ShipCity,

                      OrderDetails = new List<OrderDetails> {
                       new OrderDetails {
                           UnitPrice = od.UnitPrice,
                           Quantity = od.Quantity,
                           Discount = od.Discount,
                           Product = new Product {
                               Name = pr.Name,
                               Category = new Category {
                                   Name = c.Name
                               }
                           }
                       }
                    }
                  }).ToList();

                for (int i = 0; i < get_order_q.Count(); i++)
                {
                    Console.WriteLine(get_order_q[i].OrderDetails.First().Product.Category.Name);
                }

                return get_order_q.First();
            }
        }

        //2
        public List<Order> GetOrderFromShippingName(string shipName)
        {

            using (var db = new NordWindContext())
            {
                var ship_name_order_q =
                    from o in db.Orders
                    where o.ShipName.Equals(shipName)
                    select new Order
                    {
                        Id = o.Id,
                        Date = o.Date,
                        ShipName = o.ShipName,
                        ShipCity = o.ShipCity
                    };
                return ship_name_order_q.ToList();
            }
        }

        //3
        public List<Order> GetOrders()
        {
            using (var db = new NordWindContext())
            {
                var q = //here I became too lazy to actually call it anything other than q... sry
                    from o in db.Orders
                    select new Order
                    {
                        Id = o.Id,
                        Date = o.Date,
                        ShipName = o.ShipName,
                        ShipCity = o.ShipCity
                    };

                return q.ToList();
            }
        }

        //4
        public List<OrderDetails> GetOrderDetailsByOrderId(int id)
        {

            using (var db = new NordWindContext())
            {

                var q = //... yeah
                    (from od in db.OrderDetails
                     join pr in db.Products
                     on od.ProductId equals pr.Id
                     where od.OrderId == id
                     select new OrderDetails
                     {
                         Product = new Product
                         {
                             Name = pr.Name
                         },
                         UnitPrice = od.UnitPrice,
                         Quantity = od.Quantity
                     });

                return q.ToList();

            }
        }

        //5
        public List<OrderDetails> GetOrderDetailsByProductId(int id)
        {

            using (var db = new NordWindContext())
            {

                var q = //I know, lazy lazy lazy
                    (from od in db.OrderDetails
                     join o in db.Orders
                     on od.OrderId equals o.Id
                     where od.ProductId == id
                     select new OrderDetails
                     {
                         Order = new Order
                         {
                             Date = o.Date
                         },
                         UnitPrice = od.UnitPrice,
                         Quantity = od.Quantity
                     });

                Console.WriteLine(q.First().UnitPrice);

                return q.ToList();

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

                return query.ToList<Product>();

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
                Console.WriteLine("The category has been added");

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
                    Console.WriteLine("The category has been updated");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("The category cannot be updated");
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
                    Console.WriteLine("The category has been deleted");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("The category cannot be deleted");
                    return false;
                }
            }

        }

    }
}
