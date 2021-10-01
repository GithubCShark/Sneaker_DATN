﻿using Sneaker_DATN.Models;
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
        int GetProduct(Products product);
        int EditProduct(int id, Products product);
    }
    public class ProductSvc : IProductSvc
    {
        protected DataContext _context;
        public ProductSvc(DataContext context)
        {
            _context = context;
        }

        public List<Products> GetProductAll()
        {
            List<Products> list = new List<Products>();
            list = _context.Products.ToList();
            return list;
        }

        public Products GetProduct(int id)
        {
            Products product = null;
            product = _context.Products.Find(id); //cách này chỉ dùng cho Khóa chính
            //product = _context.Products.Where(e=>e.Id==id).FirstOrDefault(); //cách tổng quát
            return product;
        }

        public int GetProduct(Products product)
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
                _product = _context.Products.Find(id); //cách này chỉ dùng cho Khóa chính

                _product.ProductName = product.ProductName;
                _product.Price = product.Price;
                _product.Image = product.Image;
                _product.Image1 = product.Image1;
                _product.Image2 = product.Image2;
                _product.BrandID = product.BrandID;
                

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
    }
}