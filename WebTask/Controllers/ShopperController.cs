using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebTask.Data;
using WebTask.Models;

namespace WebTask.Controllers
{
    public class ShopperController : Controller
    {
        private readonly ShopperContext _context;

        public ShopperController(ShopperContext context)
        {
            _context = context;
        }

        // GET: Shopper
        public async Task<IActionResult> Index()
        {
            var shoppers = await _context.Shoppers
   .Include(s => s.ShoppingItems) // Ensure you include related data if needed
   .ToListAsync();

            return View(shoppers);
        }

        // GET: Shopper/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shoppers == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopper == null)
            {
                return NotFound();
            }

            return View(shopper);
        }

        // GET: Shopper/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shopper/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Shopper shopper)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopper);
        }

        // GET: Shopper/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shoppers == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers.FindAsync(id);
            if (shopper == null)
            {
                return NotFound();
            }
            return View(shopper);
        }

        // POST: Shopper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Shopper shopper)
        {
            if (id != shopper.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopperExists(shopper.Id))
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
            return View(shopper);
        }

        // GET: Shopper/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shoppers == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopper == null)
            {
                return NotFound();
            }

            return View(shopper);
        }

        // POST: Shopper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shoppers == null)
            {
                return Problem("Entity set 'ShopperContext.Shoppers'  is null.");
            }
            var shopper = await _context.Shoppers.FindAsync(id);
            if (shopper != null)
            {
                _context.Shoppers.Remove(shopper);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopperExists(int id)
        {
          return (_context.Shoppers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult AddItem()
        {
            // Get the list of shoppers and shopping items from the database
            var shoppers = _context.Shoppers.ToList();
            var shoppingItems = _context.ShoppingItems.ToList();

            // Create the view model
            var viewModel = new ShopperItemViewModel
            {
                Shoppers = shoppers,
                ShoppingItems = shoppingItems
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ShopperItemViewModel model)
        {
            if (model != null)
            {
                int selectedShopperId = model.SelectedShopperId;
                int selectedItemId = model.SelectedItemId;

                // Validate the selected IDs
                if (selectedShopperId == 0 || selectedItemId == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please select both a shopper and an item.");
                    // Reload the lists of shoppers and items to the view
                    model.Shoppers = _context.Shoppers.ToList();
                    model.ShoppingItems = _context.ShoppingItems.ToList();
                    return View(model);
                }

                // Retrieve the selected shopper and item from the database
                var shopper = await _context.Shoppers.FindAsync(selectedShopperId);
                var item = await _context.ShoppingItems.FindAsync(selectedItemId);

                // Check if both entities exist
                if (shopper == null || item == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid shopper or item selected.");
                    // Reload the lists of shoppers and items to the view
                    model.Shoppers = _context.Shoppers.ToList();
                    model.ShoppingItems = _context.ShoppingItems.ToList();
                    return View(model);
                }
                if (item.CurrentAvailableStock == 0)
                {
                    ModelState.AddModelError(string.Empty, "The selected item is out of stock.");
                    // Reload the lists of shoppers and items to the view
                    model.Shoppers = _context.Shoppers.ToList();
                    model.ShoppingItems = _context.ShoppingItems.ToList();
                    return View(model);
                }
                var newItem = new ShoppingItem
                {
                    Name = item.Name,
                    CurrentAvailableStock = --item.CurrentAvailableStock,
                };
                shopper.ShoppingItems.Add(newItem);
                await _context.SaveChangesAsync();
                Serialize.SerializeShoppers(_context.Shoppers.ToList());
                Serialize.SerializeShoppingItems(_context.ShoppingItems.ToList());

                return RedirectToAction("Index", "Home");
            }
            model.Shoppers = _context.Shoppers.ToList();
            model.ShoppingItems = _context.ShoppingItems.ToList();
            return View(model);
        }

        public IActionResult DeleteItem(int itemId)
        {
            // Find the item to be deleted
            var item = _context.ShoppingItems.Find(itemId);

            if (item != null)
            {
                _context.ShoppingItems.Remove(item);
                Serialize.SerializeShoppers(_context.Shoppers.ToList());
                Serialize.SerializeShoppingItems(_context.ShoppingItems.ToList());
                _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Item failed to delete.");
            }

            // Reload the view model with updated data in case of errors
            var viewModel = new ShopperItemViewModel
            {
                Shoppers = _context.Shoppers.ToList(),
                ShoppingItems = _context.ShoppingItems.ToList(),
            };
            return RedirectToAction("Index", "Home");
        }

    }
}
