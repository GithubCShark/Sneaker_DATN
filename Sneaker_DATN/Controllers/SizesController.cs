using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Sneaker_DATN.Models;

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
        public ActionResult Index(int? page)
        {
            // Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.
            // Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;
            // Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo BookID mới có thể phân trang.
            var sizes = _context.Sizes.Include(b => b.Size).OrderBy(b => b.SizeID);
            // Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 5;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            // Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            //int checkTotal = (int)(sizes.ToList().Count / pageSize) + 1;
            //// Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            //if (pageNumber > checkTotal) pageNumber = checkTotal;

            return View(_context.Sizes.ToPagedList(pageNumber, pageSize));
            //return View(await _context.Sizes.ToListAsync());
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
