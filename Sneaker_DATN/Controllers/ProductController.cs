using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DataContext _context;
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
            ViewData["brand"] = _context.Brands.ToList();
            return View(_productSvc.GetProductAll());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = _productSvc.GetProduct(id);

            var brd = _productSvc.GetBrand(product.BrandID);
            ViewData["brdetails"] = brd.BrandName;
            
            return View(product);
        }

        [BindProperty]
        public int[] selectedSize { set; get; }
        // GET: MonAnController/Create
        public ActionResult Create()
        {
            var brands = _context.Brands.ToList();
            ViewData["brands"] = new SelectList(brands, "BrandID", "BrandName");
            var sizes = _context.Sizes.ToList();
            ViewData["size"] = new MultiSelectList(sizes, "SizeID", "Size");
            var colors = _context.Colors.ToList();
            ViewData["color"] = new MultiSelectList(colors, "ColorID", "Color");
            return View();
        }

       // POST: ProductController/Create
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(Products product)
        {
            try
            {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(product.ImageFile, rootPath, "Product");
                        product.Image = product.ImageFile.FileName;                   
                }
                if (product.ImageFile1 != null && product.ImageFile1.Length > 0)
                {
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    _uploadHelper.UploadImage(product.ImageFile1, rootPath, "Product");
                    product.Image1 = product.ImageFile1.FileName;

                }
                if (product.ImageFile2 != null && product.ImageFile2.Length > 0)
                {
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    _uploadHelper.UploadImage(product.ImageFile2, rootPath, "Product");
                    product.Image2 = product.ImageFile2.FileName;

                }

                _productSvc.AddProduct(product);
                return RedirectToAction(nameof(Index), new { id = product.ProductID });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var brands = _context.Brands.ToList();
            ViewData["brands"] = new SelectList(brands, "BrandID", "BrandName");
            
            var product = _productSvc.GetProduct(id);
            return View(product);
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
                    if (product.ImageFile != null && product.ImageFile.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(product.ImageFile, rootPath, thumuccon);
                        product.Image = product.ImageFile.FileName;
                    }
                    if (product.ImageFile1 != null && product.ImageFile1.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(product.ImageFile1, rootPath, thumuccon);
                        product.Image1 = product.ImageFile1.FileName;
                    }
                    if (product.ImageFile2 != null && product.ImageFile2.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(product.ImageFile2, rootPath, thumuccon);
                        product.Image2 = product.ImageFile2.FileName;
                    }

                }
                _productSvc.EditProduct(id, product);
                return RedirectToAction(nameof(Details), new { id = product.ProductID });           
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var prod = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (prod == null)
            {
                return NotFound();
            }

            return View(prod);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
