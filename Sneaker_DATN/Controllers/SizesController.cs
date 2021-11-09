using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Sneaker_DATN.Models;
using System;

namespace Sneaker_DATN.Controllers
{
    public class SizesController : BaseController
    {
        private readonly DataContext _context;

        public SizesController(DataContext context)
        {
            _context = context;
        }

        // GET: Sizes
        public ActionResult Index(int? page, string sortOrder, string sortProperty)
        {
            if (page == null) page = 1;
            var sizes = _context.Sizes.Include(b => b.Size).OrderBy(b => b.SizeID);
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            // 1. Thêm biến NameSortParm để biết trạng thái sắp xếp tăng, giảm ở View
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";

            // 2. Truy vấn lấy tất cả đường dẫn
            var links = from l in _context.Sizes
                        select l;

            // 3. Thứ tự sắp xếp theo thuộc tính LinkName
            switch (sortOrder)
            {
                // 3.1 Nếu biến sortOrder sắp giảm thì sắp giảm theo LinkName
                case "name_desc":
                    links = links.OrderByDescending(s => s.SizeID);
                    break;
                case "Description":
                    links = links.OrderBy(s => s.Size);
                    break;
                case "description_desc":
                    links = links.OrderByDescending(s => s.Size);
                    break;

                // 3.2 Mặc định thì sẽ sắp tăng
                default:
                    links = links.OrderBy(s => s.SizeID);
                    break;
            }


            return View(links.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Sizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizes = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeID == id);
            if (sizes == null)
            {
                return NotFound();
            }

            return View(sizes);
        }

        // GET: Sizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SizeID,Size")] Sizes sizes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sizes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(sizes);
        }

        // GET: Sizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizes = await _context.Sizes.FindAsync(id);
            if (sizes == null)
            {
                return NotFound();
            }
            return View(sizes);
        }

        // POST: Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SizeID,Size")] Sizes sizes)
        {
            if (id != sizes.SizeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sizes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizesExists(sizes.SizeID))
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
            return PartialView(sizes);
        }

        // GET: Sizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizes = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeID == id);
            if (sizes == null)
            {
                return NotFound();
            }

            return View(sizes);
        }

        // POST: Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sizes = await _context.Sizes.FindAsync(id);
            _context.Sizes.Remove(sizes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizesExists(int id)
        {
            return _context.Sizes.Any(e => e.SizeID == id);
        }
    }
}
