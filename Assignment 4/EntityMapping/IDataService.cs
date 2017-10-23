using System;
using System.Collections.Generic;
using System.Text;

namespace EntityMapping
{
    public interface IDataService
    {

        Product GetProduct(int id);

        List<Product> GetProductByCategory(int id);

        List<Product> GetProductByName(string name);

    }
}
