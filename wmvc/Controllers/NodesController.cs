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
    public class NodesController : Controller
    {
        private readonly RastrwinContext _context;

        public NodesController(RastrwinContext context)
        {
            _context = context;
        }

        // GET: Nodes
        public async Task<IActionResult> Index()
        {
              return _context.Nodes != null ? 
                          View(await _context.Nodes.ToListAsync()) :
                          Problem("Entity set 'RastrwinContext.Nodes'  is null.");
        }

        // GET: Nodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nodes == null)
            {
                return NotFound();
            }

            var node = await _context.Nodes
                .FirstOrDefaultAsync(m => m.Ny == id);
            if (node == null)
            {
                return NotFound();
            }

            return View(node);
        }

        // GET: Nodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ny,Name,Na,Npa,Nsx,Sel,Sta,Tip,Uhom,Pg,Qg,Pn,Qn,Gsh,Bsh,Vzd,Qmax,Qmin,Umax,Umin,Vras,Delta,Otv,Kct,PgMax,PgMin,PgNom,Nrk,Brk,Grk,Bshr,Gshr,Psh,Qsh,StaR,Pnr,Qnr,Pgr,Qgr,Nebal,NebalQ,Nga,Dpg,Dpn,Dqn,ContrV,RegQ,Lp,Lq,NaName,NaPop,Epn,Epg,Esh,Ekn,Ysh,Sn,Sg,Uc,Ssh,Qmima,Umima,Najact,NaNo,Nadjgen,Numgen,Snr,Sgr,Nf,Muskod,Dqmin,Dqmax,PnMin,PnMax,Sn1,Tsn,TgPhi,QnMin,QnMax,ExistLoad,ExistGen,BasePriority,BaseArea,NablP,NablQ,NablPFrag,NablQFrag,PnIzm,QnIzm,UIzm,TiPn,TiQn,TiPg,TiQg,TiSn,TiSg,TiVras,TiPnDiff,TiQnDiff,TiPgDiff,TiQgDiff,TiVrasDiff,TiBalP,TiBalQ,TiBalPq,TiSnDiff,TiSgDiff,NySubst,Supernode,Rang,TiOcSn,TiOcSg,TiOcVras,Numload,Nadjload,NablPGen")] Node node)
        {
            if (ModelState.IsValid)
            {
                _context.Add(node);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(node);
        }

        // GET: Nodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nodes == null)
            {
                return NotFound();
            }

            var node = await _context.Nodes.FindAsync(id);
            if (node == null)
            {
                return NotFound();
            }
            return View(node);
        }

        // POST: Nodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ny,Name,Na,Npa,Nsx,Sel,Sta,Tip,Uhom,Pg,Qg,Pn,Qn,Gsh,Bsh,Vzd,Qmax,Qmin,Umax,Umin,Vras,Delta,Otv,Kct,PgMax,PgMin,PgNom,Nrk,Brk,Grk,Bshr,Gshr,Psh,Qsh,StaR,Pnr,Qnr,Pgr,Qgr,Nebal,NebalQ,Nga,Dpg,Dpn,Dqn,ContrV,RegQ,Lp,Lq,NaName,NaPop,Epn,Epg,Esh,Ekn,Ysh,Sn,Sg,Uc,Ssh,Qmima,Umima,Najact,NaNo,Nadjgen,Numgen,Snr,Sgr,Nf,Muskod,Dqmin,Dqmax,PnMin,PnMax,Sn1,Tsn,TgPhi,QnMin,QnMax,ExistLoad,ExistGen,BasePriority,BaseArea,NablP,NablQ,NablPFrag,NablQFrag,PnIzm,QnIzm,UIzm,TiPn,TiQn,TiPg,TiQg,TiSn,TiSg,TiVras,TiPnDiff,TiQnDiff,TiPgDiff,TiQgDiff,TiVrasDiff,TiBalP,TiBalQ,TiBalPq,TiSnDiff,TiSgDiff,NySubst,Supernode,Rang,TiOcSn,TiOcSg,TiOcVras,Numload,Nadjload,NablPGen")] Node node)
        {
            if (id != node.Ny)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(node);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NodeExists(node.Ny))
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
            return View(node);
        }

        // GET: Nodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nodes == null)
            {
                return NotFound();
            }

            var node = await _context.Nodes
                .FirstOrDefaultAsync(m => m.Ny == id);
            if (node == null)
            {
                return NotFound();
            }

            return View(node);
        }

        // POST: Nodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nodes == null)
            {
                return Problem("Entity set 'RastrwinContext.Nodes'  is null.");
            }
            var node = await _context.Nodes.FindAsync(id);
            if (node != null)
            {
                _context.Nodes.Remove(node);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NodeExists(int id)
        {
          return (_context.Nodes?.Any(e => e.Ny == id)).GetValueOrDefault();
        }
    }
}
