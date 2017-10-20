using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityMapping
{
    public class Product
    {

        [Column("productid")]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryID { get; set; }

        public double UnitPrice { get; set; }

        public string QuantityUnit { get; set; }

        public int UnitsInStock { get; set; }

    }
}
