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
using X.PagedList;

namespace Sneaker_DATN.Controllers
{
    //[Route("/product")]
    public class ProductController : BaseController
    {
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DataContext _context;
        private IProductSvc _productSvc;
        private IUploadHelper _uploadHelper;
        public ProductController(IWebHostEnvironment webHostEnviroment, 
            IProductSvc productSvc, IUploadHelper uploadHelper, DataContext context)
        {
            _webHostEnvironment = webHostEnviroment;
            _productSvc = productSvc;
            _uploadHelper = uploadHelper;
            _context = context;
        }

        // GET: ProductController
        public ActionResult Index(int? page)
        {
            // Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.
            // Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;
            // Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo BookID mới có thể phân trang.
            var products = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);
            // Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 4;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            // Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            //int checkTotal = (int)(sizes.ToList().Count / pageSize) + 1;
            //// Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            //if (pageNumber > checkTotal) pageNumber = checkTotal;
            ViewData["brand"] = _context.Brands.ToList();
            return View(_context.Products.ToPagedList(pageNumber, pageSize));
            //return View(await _context.Sizes.ToListAsync());

            //ViewData["brand"] = _context.Brands.ToList();
            //ViewData["sz"] = _context.Sizes.ToList();
            //ViewData["clo"] = _context.ProductColors.ToList();
            //return View(_productSvc.GetProductAll());
        }

        //[BindProperty]
        //public string[] selectedSize { set; get; }
        // GET: MonAnController/Create
        public ActionResult Create()
        {
            var brands = _context.Brands.ToList();
            ViewData["brands"] = new SelectList(brands, "BrandID", "BrandName");
            
            var colors = _context.Colors.ToList();
            ViewData["color"] = new MultiSelectList(colors, "ColorID", "Color");

            //ViewData["size"] = _context.Sizes.ToList();
            ViewData["size"] = (from c in _context.Sizes
                                select new Sizes()
                                {
                                    Size = c.Size,
                                    SizeID = c.SizeID
                                }).ToList();
            //IEnumerable<Sizes> listSize = (from c in _context.Sizes
            //                               select new Sizes()
            //                               {
            //                                     Size = c.Size,
            //                                     SizeID = c.SizeID
            //                               }).ToList();
            return View();
        }

       // POST: ProductController/Create
       [HttpPost]
       [ValidateAntiForgeryToken]
        public  IActionResult Create(Products product,List<int> ListSize)   
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
                //var prod = await _context.ProductSizes.FindAsync(product.ProductID);
                //_context.ProductSizes.Remove(prod);
                //foreach (var selectedSize in selectedSize)
                //{                    
                //    _context.Add(new ProductSize() { ProductID = product.ProductID, SizeID = int.Parse(selectedSize) });
                //}

                //await _context.SaveChangesAsync();
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

            //var sizes = _context.Sizes.ToList();
            //ViewData["size"] = new MultiSelectList(sizes, "SizeID", "Size");

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
        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = _productSvc.GetProduct(id);

            var brd = _productSvc.GetBrand(product.BrandID);
            ViewData["brdetails"] = brd.BrandName;

            //var brcl = _productSvc.GetColor(product.ColorID);
            //ViewData["brcl"] = brcl.Color;

            return View(product);
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

        //public ActionResult AddOrEdit(int id = 0)
        //{
        //    ProductSize productSize = new ProductSize();
        //    using (_context)
        //    {
        //        if (id != 0)
        //        {
        //            productSize = _context.ProductSizes.Where(x => x.ProductID == id).FirstOrDefault();
        //            //mutiple select
        //            productSize.SelectSize = productSize.SelectProSize.Split(',').ToArray();
        //        }
        //        productSize.SizesProperty = _context.Sizes.ToList();
        //    }
        //    return View(productSize);
        //}
        //[HttpPost]
        //public ActionResult AddOrEdit(ProductSize pro)
        //{
        //    pro.SelectProSize = string.Join(",", pro.SelectSize);
        //    using (_context)
        //    {
        //        if (pro.ProductID == 0)
        //        {
        //            _context.ProductSizes.Add(pro);
        //        }
        //        else
        //        {
        //            _context.Entry(pro).State = EntityState.Modified;
        //        }
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("AddOrEdit", new { id = 0 });
        //}
    }
}
