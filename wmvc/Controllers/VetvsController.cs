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
    public class VetvsController : Controller
    {
        private readonly RastrwinContext _context;

        public VetvsController(RastrwinContext context)
        {
            _context = context;
        }

        // GET: Vetvs
        public async Task<IActionResult> Index()
        {
            var rastrwinContext = _context.Vetvs.Include(v => v.BdNavigation).Include(v => v.IpNavigation).Include(v => v.IqNavigation);
            return View(await rastrwinContext.ToListAsync());
        }

        // GET: Vetvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vetvs == null)
            {
                return NotFound();
            }

            var vetv = await _context.Vetvs
                .Include(v => v.BdNavigation)
                .Include(v => v.IpNavigation)
                .Include(v => v.IqNavigation)
                .FirstOrDefaultAsync(m => m.Ip == id);
            if (vetv == null)
            {
                return NotFound();
            }

            return View(vetv);
        }

        // GET: Vetvs/Create
        public IActionResult Create()
        {
            ViewData["Bd"] = new SelectList(_context.Ancapfs, "Nbd", "Nbd");
            ViewData["Ip"] = new SelectList(_context.Nodes, "Ny", "Ny");
            ViewData["Iq"] = new SelectList(_context.Nodes, "Ny", "Ny");
            return View();
        }

        // POST: Vetvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ip,Iq,Np,Sel,Sta,Tip,R,X,B,G,Ktr,KrMax,KrMin,Bd,NAnc,Na,Div,Npa,Div2,Nga,Div3,IDop,Msi,IMsi,Kti,KiMax,KiMin,NrIp,SrIp,BrIp,GrIp,NrIq,SrIq,BrIq,GrIq,BIp,BIq,GIp,GIq,PlIp,QlIp,PlIq,QlIq,VIp,VIq,DIp,DIq,Dp,Dq,Ib,Ie,Psh,Qsh,Name,Dname,RegKt,ContrI,Z,Slb,Sle,Zbg,Zen,Tmpny,NameNy,InNy,PlNy,QlNy,INy,NaNy,NaName,NaPl,NaNa,NaDp,ZagI,ZagIt,NaDv,KtB,Dv,Dij,DvNy,DijNy,NpaNa,NpaNy,NpaName,NpaPl,NpaDp,NpaDv,Plmax,Slmax,Sup,Sup2,SignP,SignQip,SignQiq,V2Ny,PlBal,IDopR,NIt,Tc,IDopOb,Groupid,NAncI,Ta,MaxDd,MaxDv,IsBreaker,IMax,IZag,DivNga,NgaNa,NgaNy,NgaName,NgaPl,NgaDp,NgaDv,TiPlIp,TiPlIq,TiQlIp,TiQlIq,TiV2Ny,TiPlIpNy,TiQlIpNy,TiPlIqNy,TiQlIqNy,TiPlIpNyDiff,TiQlIpNyDiff,TiPlIqNyDiff,TiQlIqNyDiff,PlIpNy,QlIpNy,PlIqNy,QlIqNy,TiSlb,TiSle,TiVrasDiff,Brand,L,NItAv,IDopAv,IDopObAv,IDopRAv,ZagIAv,TiSlbDiff,TiSleDiff,L2,L3,Brand2,Brand3,ZagItAv,MeteoIdIp,MeteoIdIq,StrMeteoIdIp,StrMeteoIdIq,SupernodeIp,SupernodeIq,Lsum,TiOcPlIpNy,TiOcPlIqNy,TiOcQlIpNy,TiOcQlIqNy,TiOcSlb,TiOcSle")] Vetv vetv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vetv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Bd"] = new SelectList(_context.Ancapfs, "Nbd", "Nbd", vetv.Bd);
            ViewData["Ip"] = new SelectList(_context.Nodes, "Ny", "Ny", vetv.Ip);
            ViewData["Iq"] = new SelectList(_context.Nodes, "Ny", "Ny", vetv.Iq);
            return View(vetv);
        }

        // GET: Vetvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vetvs == null)
            {
                return NotFound();
            }

            var vetv = await _context.Vetvs.FindAsync(id);
            if (vetv == null)
            {
                return NotFound();
            }
            ViewData["Bd"] = new SelectList(_context.Ancapfs, "Nbd", "Nbd", vetv.Bd);
            ViewData["Ip"] = new SelectList(_context.Nodes, "Ny", "Ny", vetv.Ip);
            ViewData["Iq"] = new SelectList(_context.Nodes, "Ny", "Ny", vetv.Iq);
            return View(vetv);
        }

        // POST: Vetvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ip,Iq,Np,Sel,Sta,Tip,R,X,B,G,Ktr,KrMax,KrMin,Bd,NAnc,Na,Div,Npa,Div2,Nga,Div3,IDop,Msi,IMsi,Kti,KiMax,KiMin,NrIp,SrIp,BrIp,GrIp,NrIq,SrIq,BrIq,GrIq,BIp,BIq,GIp,GIq,PlIp,QlIp,PlIq,QlIq,VIp,VIq,DIp,DIq,Dp,Dq,Ib,Ie,Psh,Qsh,Name,Dname,RegKt,ContrI,Z,Slb,Sle,Zbg,Zen,Tmpny,NameNy,InNy,PlNy,QlNy,INy,NaNy,NaName,NaPl,NaNa,NaDp,ZagI,ZagIt,NaDv,KtB,Dv,Dij,DvNy,DijNy,NpaNa,NpaNy,NpaName,NpaPl,NpaDp,NpaDv,Plmax,Slmax,Sup,Sup2,SignP,SignQip,SignQiq,V2Ny,PlBal,IDopR,NIt,Tc,IDopOb,Groupid,NAncI,Ta,MaxDd,MaxDv,IsBreaker,IMax,IZag,DivNga,NgaNa,NgaNy,NgaName,NgaPl,NgaDp,NgaDv,TiPlIp,TiPlIq,TiQlIp,TiQlIq,TiV2Ny,TiPlIpNy,TiQlIpNy,TiPlIqNy,TiQlIqNy,TiPlIpNyDiff,TiQlIpNyDiff,TiPlIqNyDiff,TiQlIqNyDiff,PlIpNy,QlIpNy,PlIqNy,QlIqNy,TiSlb,TiSle,TiVrasDiff,Brand,L,NItAv,IDopAv,IDopObAv,IDopRAv,ZagIAv,TiSlbDiff,TiSleDiff,L2,L3,Brand2,Brand3,ZagItAv,MeteoIdIp,MeteoIdIq,StrMeteoIdIp,StrMeteoIdIq,SupernodeIp,SupernodeIq,Lsum,TiOcPlIpNy,TiOcPlIqNy,TiOcQlIpNy,TiOcQlIqNy,TiOcSlb,TiOcSle")] Vetv vetv)
        {
            if (id != vetv.Ip)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vetv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VetvExists(vetv.Ip))
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
            ViewData["Bd"] = new SelectList(_context.Ancapfs, "Nbd", "Nbd", vetv.Bd);
            ViewData["Ip"] = new SelectList(_context.Nodes, "Ny", "Ny", vetv.Ip);
            ViewData["Iq"] = new SelectList(_context.Nodes, "Ny", "Ny", vetv.Iq);
            return View(vetv);
        }

        // GET: Vetvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vetvs == null)
            {
                return NotFound();
            }

            var vetv = await _context.Vetvs
                .Include(v => v.BdNavigation)
                .Include(v => v.IpNavigation)
                .Include(v => v.IqNavigation)
                .FirstOrDefaultAsync(m => m.Ip == id);
            if (vetv == null)
            {
                return NotFound();
            }

            return View(vetv);
        }

        // POST: Vetvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vetvs == null)
            {
                return Problem("Entity set 'RastrwinContext.Vetvs'  is null.");
            }
            var vetv = await _context.Vetvs.FindAsync(id);
            if (vetv != null)
            {
                _context.Vetvs.Remove(vetv);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VetvExists(int id)
        {
          return (_context.Vetvs?.Any(e => e.Ip == id)).GetValueOrDefault();
        }
    }
}
