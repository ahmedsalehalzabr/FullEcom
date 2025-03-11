using Ecom.Core.Dto;
using Ecom.Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.interfaces
{
    public interface IProductRepositry:IGenericRepositry<Product>
    {
        Task<bool> AddAsync(AddProductDto addProductDto);
        Task<bool> UpdateAsync(UpdateProudactDto updateProudactDto);
        Task<bool> DeleteAsync(Product product);
    }
}
