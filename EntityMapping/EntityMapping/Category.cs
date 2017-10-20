using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityMapping
{

    public class Category
    {
        [Column("categoryid")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
