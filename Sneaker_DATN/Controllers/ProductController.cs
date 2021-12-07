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
        public ActionResult Index(string sortOrder, bool status, string currentFilter, string searchString, int? page, bool currentFilterStatus)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.SaleSortParm = sortOrder == "sale" ? "sale_desc" : "sale";

            ViewBag.Status = (from r in _productSvc.GetProductAll()
                              select r.Status).Distinct();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (status)
            {
                page = 1;
            }
            else
            {
                status = currentFilterStatus;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentFilterStatus = status;

            var students = from s in _context.Products
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }
            else if(!String.IsNullOrEmpty(status.ToString()))
            {
                students = students.Where(s => s.Status == status);
            }
            switch (sortOrder)
            {
                case "id_desc":
                    students = students.OrderBy(s => s.ProductID);
                    break;

                case "name":
                    students = students.OrderBy(s => s.ProductName);
                    break;
                case "name_desc":
                    students = students.OrderByDescending(s => s.ProductName);
                    break;

                case "price":
                    students = students.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    students = students.OrderByDescending(s => s.Price);
                    break;

                case "sale":
                    students = students.OrderBy(s => s.Sale);
                    break;
                case "sale_desc":
                    students = students.OrderByDescending(s => s.Sale);
                    break;

                default:
                    students = students.OrderByDescending(s => s.ProductID);
                    break;
            }

            if (page == null) page = 1;

            var products = _context.Products.Include(b => b.ProductName)
                .OrderBy(b => b.ProductID);

            int pageSize = 6;

            int pageNumber = (page ?? 1);


            ViewBag.productSize = _context.ProductSizes.ToList();
            ViewData["size"] = _context.Sizes.ToList();

            ViewBag.productColor = _context.ProductColors.ToList();
            ViewData["colors"] = _context.Colors.ToList();

            ViewData["brand"] = _context.Brands.ToList();
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        [BindProperty]
        public int[] selectedSize { get; set; }
        [BindProperty]
        public int[] selectedColor { get; set; }
        // GET: MonAnController/Create
        public ActionResult Create()
        {
            var brands = _context.Brands.ToList();
            ViewData["brands"] = new SelectList(brands, "BrandID", "BrandName");

            // Danh mục chọn để đăng bài Post
            var ProCategory = _context.Sizes.ToList();
            ViewData["categories"] = new MultiSelectList(ProCategory, "SizeID", "Size");

            var catecolor = _context.Colors.ToList();
            ViewData["color"] = new MultiSelectList(catecolor, "ColorID", "Color");

            return PartialView();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Products product, [Bind("SizeID, Size")] ProductSize post, [Bind("ColorID, Color")] ProductColor col)
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

                if (ModelState.IsValid)
                {
                    foreach (var selectedCategory in selectedSize)
                    {
                        _context.Add(new ProductSize() { ID = product.ProductID, IdSize = selectedCategory });
                    }
                    _context.SaveChanges();

                    var ProCategory = _context.Sizes.ToList();
                    ViewData["categories"] = new MultiSelectList(ProCategory, "SizeID", "Size");
                }
                if (ModelState.IsValid)
                {
                    foreach (var selectedCateColor in selectedColor)
                    {
                        _context.Add(new ProductColor() { ID = product.ProductID, ColorID = selectedCateColor });
                    }
                    _context.SaveChanges();

                    var catecolor = _context.Colors.ToList();
                    ViewData["color"] = new MultiSelectList(catecolor, "ColorID", "Color");
                }
                return RedirectToAction(nameof(Index));

            }
            catch
            {

                return PartialView();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {

            var post = _context.Products.Where(p => p.ProductID == id)
                        .Include(p => p.ProductSizes)
                        .ThenInclude(c => c.Sizes)
                        .Include(p => p.ProductColors)
                        .ThenInclude(c => c.Colors).FirstOrDefault();
            var brands = _context.Brands.ToList();
            ViewData["brands"] = new SelectList(brands, "BrandID", "BrandName");

            var selectedCates = post.ProductSizes.Select(c => c.IdSize).ToArray();
            var ProCategory = _context.Sizes.ToList();
            ViewData["categories"] = new MultiSelectList(ProCategory, "SizeID", "Size", selectedCates);

            var selectedCatesCl = post.ProductColors.Select(c => c.ColorID).ToArray();
            var catecolor = _context.Colors.ToList();
            ViewData["color"] = new MultiSelectList(catecolor, "ColorID", "Color", selectedCatesCl);

            var product = _productSvc.GetProduct(id);

            return PartialView(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Products product, [Bind("SizeID, Size")] ProductSize prosize, [Bind("ColorID, Color")] ProductColor procolor)
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

                if (ModelState.IsValid)
                {
                    // Lấy nội dung từ DB
                    var postUpdate = _context.Products.Where(p => p.ProductID == id)
                        .Include(p => p.ProductSizes)
                        .ThenInclude(c => c.Sizes)
                        .Include(p => p.ProductColors)
                        .ThenInclude(c => c.Colors).FirstOrDefault();
                    if (postUpdate == null)
                    {
                        return NotFound();
                    }

                    // Các danh mục không có trong selected
                    //Size
                    var listcateremove = postUpdate.ProductSizes
                                                   .Where(p => !selectedSize.Contains(p.IdSize))
                                                   .ToList();
                    listcateremove.ForEach(c => postUpdate.ProductSizes.Remove(c));
                    //color
                    var listcateremovecl = postUpdate.ProductColors
                                                   .Where(p => !selectedColor.Contains(p.ColorID))
                                                   .ToList();
                    listcateremovecl.ForEach(c => postUpdate.ProductColors.Remove(c));

                    // Các ID category chưa có trong postUpdate.Productsize and color
                    //Size
                    var listCateAdd = selectedSize
                                        .Where(
                                            id => !postUpdate.ProductSizes.Where(c => c.IdSize == id).Any()
                                        ).ToList();

                    listCateAdd.ForEach(id =>
                    {
                        postUpdate.ProductSizes.Add(new ProductSize()
                        {
                            ID = postUpdate.ProductID,
                            IdSize = id
                        });
                    });
                    //Color
                    var listCateAddcl = selectedColor
                                        .Where(
                                            id => !postUpdate.ProductColors.Where(c => c.ColorID == id).Any()
                                        ).ToList();

                    listCateAddcl.ForEach(id =>
                    {
                        postUpdate.ProductColors.Add(new ProductColor()
                        {
                            ID = postUpdate.ProductID,
                            ColorID = id
                        });
                    });
                    _context.Update(postUpdate);
                    _context.SaveChanges();

                    var ProCategory = _context.Sizes.ToList();
                    ViewData["categories"] = new MultiSelectList(ProCategory, "SizeID", "Size");

                    var catecolor = _context.Colors.ToList();
                    ViewData["color"] = new MultiSelectList(catecolor, "ColorID", "Color");
                }
                _productSvc.EditProduct(id, product);
                return RedirectToAction(nameof(Index), new { id = product.ProductID });
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

            ViewBag.productSize = _context.ProductSizes.ToList();
            ViewData["size"] = _context.Sizes.ToList();

            ViewBag.productColor = _context.ProductColors.ToList();
            ViewData["colors"] = _context.Colors.ToList();
            return PartialView(product);
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

            return PartialView(prod);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            var order = from i in _context.OrderDetails
                              group i by i.ProductID into j
                              select new { ID = j.Key };
            bool temp = false;
            foreach (var item in order)
            {
                if( item.ID == id)
                {
                    temp = true;
                    break;
                }
            }
            if(!temp)
            {
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
            }
            else
            {
                prod.Status = true;
                _context.Products.Update(prod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
