using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sneaker_DATN.Models;
using X.PagedList;

namespace Sneaker_DATN.Controllers
{
    public class ColorsController : BaseController
    {
        private readonly DataContext _context;

        public ColorsController(DataContext context)
        {
            _context = context;
        }

        // GET: Colors
        public ActionResult Index(int? page, string sortOrder, string sortProperty)
        {
            if (page == null) page = 1;
            var color = _context.Colors.Include(b => b.Color).OrderBy(b => b.ColorID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";

            // 2. Truy vấn lấy tất cả đường dẫn
            var links = from l in _context.Colors
                        select l;

            // 3. Thứ tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                // 3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "name_desc":
                    links = links.OrderByDescending(s => s.ColorID);
                    break;
                case "Description":
                    links = links.OrderBy(s => s.Color);
                    break;
                case "description_desc":
                    links = links.OrderByDescending(s => s.Color);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    links = links.OrderBy(s => s.ColorID);
                    break;
            }


            return View(links.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Colors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorID == id);
            if (colors == null)
            {
                return NotFound();
            }

            return View(colors);
        }

        // GET: Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColorID,Color")] Colors colors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(colors);
        }

        // GET: Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }
            return View(colors);
        }

        // POST: Colors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColorID,Color")] Colors colors)
        {
            if (id != colors.ColorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorsExists(colors.ColorID))
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
            return PartialView(colors);
        }

        // GET: Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorID == id);
            if (colors == null)
            {
                return NotFound();
            }

            return View(colors);
        }

        // POST: Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colors = await _context.Colors.FindAsync(id);
            _context.Colors.Remove(colors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorsExists(int id)
        {
            return _context.Colors.Any(e => e.ColorID == id);
        }
    }
}
