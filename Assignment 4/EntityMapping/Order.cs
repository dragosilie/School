using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityMapping
{
    public class Order
    {

        [Column("orderid")]
        public int Id { get; set; }

        [Column("orderdate")]
        public DateTime Date { get; set; } //Using string as date

        public DateTime Required { get; set; }

        public DateTime Shipped { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipCity { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

    }
}
