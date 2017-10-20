using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityMapping
{
    class Order
    {

        [Column("orderid")]
        public int Id { get; set; }

        [Column("orderdate")]
        public DateTime Date { get; set; } //Using string as date

        public string Require { get; set; }

        public string Shipped { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipCity { get; set; }

    }
}
