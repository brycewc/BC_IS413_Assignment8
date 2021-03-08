using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC_IS413_Assignment8.Models;
using Microsoft.AspNetCore.Mvc;

namespace BC_IS413_Assignment8.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;
        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }
        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
