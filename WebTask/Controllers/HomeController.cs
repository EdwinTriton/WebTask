using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTask.Models;
using WebTask.Data;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ShopperContext _context;

    public HomeController(ShopperContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {

        var shoppers = _context.Shoppers.Include(s => s.ShoppingItems).ToList();
        var viewModel = new HomePageViewModel
        {
            Shoppers = shoppers,
            ShoppingItems = _context.ShoppingItems.ToList() 
        };

        return View(viewModel);
    }
}
