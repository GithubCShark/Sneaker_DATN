using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sneaker_DATN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Sneaker_DATN.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly DataContext _context;

        public BrandsController(DataContext context)
        {
            _context = context;
        }
        // GET: BrandController
        public ActionResult Index(int? page, string sortOrder, string sortProperty)
        {
            if (page == null) page = 1;
            var brand = _context.Brands.Include(b => b.BrandName).OrderBy(b => b.BrandID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";

            //2. Truy vấn lấy tất cả đường dẫn
            var links = from l in _context.Brands
                        select l;

            //3. Thú tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                //3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "name_desc":
                    links = links.OrderByDescending(s => s.BrandID);
                    break;
                case "Description":
                    links = links.OrderBy(s => s.BrandName);
                    break;
                case "description_desc":
                    links = links.OrderByDescending(s => s.BrandName);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    links = links.OrderBy(s => s.BrandID);
                    break;
            }

            return View(links.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: BrandController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandID == id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }

        // GET: BrandController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandID,BrandName")] Brands brands)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brands);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(brands);
        }

        // GET: BrandController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandID,BrandName")] Brands brands)
        {
            if (id != brands.BrandID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brands);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandsExists(brands.BrandID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView(brands);
        }

        // GET: BrandController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandID == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // POST: BrandController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brands = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brands);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BrandsExists(int id)
        {
            return _context.Brands.Any(e => e.BrandID == id);
        }
    }
}
