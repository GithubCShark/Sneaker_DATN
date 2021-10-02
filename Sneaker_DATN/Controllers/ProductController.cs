using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Controllers
{
    //[Route("/product")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IProductSvc _productSvc;
        private IUploadHelper _uploadHelper;
        public ProductController(IWebHostEnvironment webHostEnviroment, IProductSvc productSvc, IUploadHelper uploadHelper, DataContext context)
        {
            _webHostEnvironment = webHostEnviroment;
            _productSvc = productSvc;
            _uploadHelper = uploadHelper;
            _context = context;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            return View(_productSvc.GetProductAll());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = _productSvc.GetProduct(id);
            return View(product);
        }

        // GET: MonAnController/Create
        public ActionResult Create()
        {
            var brands = _context.Brands.ToList();
            ViewData["brands"] = new SelectList(brands, "BrandID", "BrandName");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products product)
        {
            try
            {
                if (product.ImageFile != null)
                {
                    if (product.ImageFile.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(product.ImageFile, rootPath, "Product");
                        product.Image = product.ImageFile.FileName;
                        product.Image1 = product.ImageFile1.FileName;
                        product.Image2 = product.ImageFile2.FileName;

                    }

                }
               
                _productSvc.AddProduct(product);
                return RedirectToAction(nameof(Details), new { id = product.ProductID });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var monAn = _productSvc.GetProduct(id);
            return View(monAn);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Products product)
        {
            string thumuccon = "Product";
            try
            {
                if (ModelState.IsValid)
                {
                    if (product.ImageFile != null)
                    {
                        if (product.ImageFile.Length > 0)
                        {
                            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                            //_uploadHelper.RemoveImage(rootPath + @"\monan\" + monAn.Hinh);
                            _uploadHelper.UploadImage(product.ImageFile, rootPath, thumuccon);
                            product.Image = product.ImageFile.FileName;
                            product.Image1 = product.ImageFile.FileName;
                            product.Image2 = product.ImageFile.FileName;
                        }
                    }
                    _productSvc.EditProduct(id, product);
                    return RedirectToAction(nameof(Details), new { id = product.ProductID });
                }
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
