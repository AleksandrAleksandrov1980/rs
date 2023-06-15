using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wmvc.Models;

namespace wmvc.Controllers
{
    public class ComOpfsController : Controller
    {
        private readonly RastrwinContext _context;

        public ComOpfsController(RastrwinContext context)
        {
            _context = context;
        }

        // GET: ComOpfs
        public async Task<IActionResult> Index()
        {
              return _context.ComOpfs != null ? 
                          View(await _context.ComOpfs.ToListAsync()) :
                          Problem("Entity set 'RastrwinContext.ComOpfs'  is null.");
        }

        // GET: ComOpfs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ComOpfs == null)
            {
                return NotFound();
            }

            var comOpf = await _context.ComOpfs
                .FirstOrDefaultAsync(m => m.Num == id);
            if (comOpf == null)
            {
                return NotFound();
            }

            return View(comOpf);
        }

        // GET: ComOpfs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComOpfs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Centr,Tipr,ItMax,Plos,Status,Potr,LpMax,LqMax,LdpgMax,LpsMax,LiMax,LvMax,LdqMax,LktMax,LpDta,LqDta,LdpgDta,LpsDta,LiDta,LvDta,LdqDta,LktDta,OutLevel,LioMax,LioDta,TFunc,TgFunc,Tarif,StartMethod,MainMethod,Criteria1,Formula1,RIt,StartSigma,MainSigma,RRo,RSigma,RPdad,RPdadAf,MinMu2,MinMu")] ComOpf comOpf)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comOpf);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comOpf);
        }

        // GET: ComOpfs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ComOpfs == null)
            {
                return NotFound();
            }

            var comOpf = await _context.ComOpfs.FindAsync(id);
            if (comOpf == null)
            {
                return NotFound();
            }
            return View(comOpf);
        }

        // POST: ComOpfs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Num,Centr,Tipr,ItMax,Plos,Status,Potr,LpMax,LqMax,LdpgMax,LpsMax,LiMax,LvMax,LdqMax,LktMax,LpDta,LqDta,LdpgDta,LpsDta,LiDta,LvDta,LdqDta,LktDta,OutLevel,LioMax,LioDta,TFunc,TgFunc,Tarif,StartMethod,MainMethod,Criteria1,Formula1,RIt,StartSigma,MainSigma,RRo,RSigma,RPdad,RPdadAf,MinMu2,MinMu")] ComOpf comOpf)
        {
            if (id != comOpf.Num)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comOpf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComOpfExists(comOpf.Num))
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
            return View(comOpf);
        }

        // GET: ComOpfs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ComOpfs == null)
            {
                return NotFound();
            }

            var comOpf = await _context.ComOpfs
                .FirstOrDefaultAsync(m => m.Num == id);
            if (comOpf == null)
            {
                return NotFound();
            }

            return View(comOpf);
        }

        // POST: ComOpfs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ComOpfs == null)
            {
                return Problem("Entity set 'RastrwinContext.ComOpfs'  is null.");
            }
            var comOpf = await _context.ComOpfs.FindAsync(id);
            if (comOpf != null)
            {
                _context.ComOpfs.Remove(comOpf);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComOpfExists(int id)
        {
          return (_context.ComOpfs?.Any(e => e.Num == id)).GetValueOrDefault();
        }
    }
}
