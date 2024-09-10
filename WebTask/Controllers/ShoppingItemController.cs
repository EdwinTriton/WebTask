using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebTask.Data;
using WebTask.Models;

namespace WebTask.Controllers
{
    public class ShoppingItemController : Controller
    {
        private readonly ShopperContext _context;

        public ShoppingItemController(ShopperContext context)
        {
            _context = context;
        }

        // GET: ShoppingItem
        public async Task<IActionResult> Index()
        {
              return _context.ShoppingItems != null ? 
                          View(await _context.ShoppingItems.ToListAsync()) :
                          Problem("Entity set 'ShopperContext.ShoppingItems'  is null.");
        }

        // GET: ShoppingItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingItems == null)
            {
                return NotFound();
            }

            var shoppingItem = await _context.ShoppingItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingItem == null)
            {
                return NotFound();
            }

            return View(shoppingItem);
        }

        // GET: ShoppingItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CurrentAvailableStock")] ShoppingItem shoppingItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingItem);
        }

        // GET: ShoppingItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingItems == null)
            {
                return NotFound();
            }

            var shoppingItem = await _context.ShoppingItems.FindAsync(id);
            if (shoppingItem == null)
            {
                return NotFound();
            }
            return View(shoppingItem);
        }

        // POST: ShoppingItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CurrentAvailableStock")] ShoppingItem shoppingItem)
        {
            if (id != shoppingItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingItemExists(shoppingItem.Id))
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
            return View(shoppingItem);
        }

        // GET: ShoppingItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingItems == null)
            {
                return NotFound();
            }

            var shoppingItem = await _context.ShoppingItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingItem == null)
            {
                return NotFound();
            }

            return View(shoppingItem);
        }

        // POST: ShoppingItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingItems == null)
            {
                return Problem("Entity set 'ShopperContext.ShoppingItems'  is null.");
            }
            var shoppingItem = await _context.ShoppingItems.FindAsync(id);
            if (shoppingItem != null)
            {
                _context.ShoppingItems.Remove(shoppingItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingItemExists(int id)
        {
          return (_context.ShoppingItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
