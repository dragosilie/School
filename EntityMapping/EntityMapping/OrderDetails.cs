using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityMapping
{
    public class OrderDetails
    {

        [Key, Column("OrderID", Order = 0)]
        public int OrderID { get; set; }

        [Key, Column("ProductID", Order = 1)]
        public int ProductID { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }


    }
}
