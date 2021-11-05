using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IProductSvc
    {
        List<Products> GetProductAll();

        Products GetProduct(int id);

        int AddProduct(Products product);

        int EditProduct(int id, Products product);

        Brands GetBrand(int id);
        //ProductSize GetSizes(int id);
        //ProductColor GetColors(int id);

        List<Products> SneakerSearchString(string name);
    }
    public class ProductSvc : IProductSvc
    {
        protected DataContext _context;
        public ProductSvc(DataContext context)
        {
            _context = context;
        }

        public List<Products> SneakerSearchString(string name)
        {
            return _context.Products.Where(x => x.ProductName.Contains(name.Trim())).ToList();
        }

        public List<Products> GetProductAll()
        {
            List<Products> list = new List<Products>();
            list = _context.Products.ToList();
            //List<ProductSize> listsize = new List<ProductSize>();
            //listsize = _context.ProductSizes.ToList();
            //List<ProductColor> listcolor = new List<ProductColor>();
            //listcolor = _context.ProductColors.ToList();
            return list;
        }

        public Products GetProduct(int id)
        {
            Products product = null;
            product = _context.Products.Find(id);
            return product;
        }
        public int AddProduct(Products product)
        {
            int ret = 0;
            try
            {
                _context.Add(product);
                _context.SaveChanges();
                ret = product.ProductID;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public int EditProduct(int id, Products product)
        {
            int ret = 0;
            try
            {
                Products _product = null;

                _product = _context.Products.Find(id);

                _product.ProductName = product.ProductName;
                _product.Price = product.Price;
                _product.Image = product.Image;
                _product.Image1 = product.Image1;
                _product.Image2 = product.Image2;
                _product.ImageFile = product.ImageFile;
                _product.ImageFile1 = product.ImageFile1;
                _product.ImageFile2 = product.ImageFile2;
                _product.BrandID = product.BrandID;
                _product.Status = product.Status;
                _product.Description = product.Description;
                _context.Update(_product);

                _context.SaveChanges();

                ret = product.ProductID;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public Brands GetBrand(int id)
        {
            Brands brd = null;
            brd = _context.Brands.Find(id);
            return brd;
        }
    }
}
