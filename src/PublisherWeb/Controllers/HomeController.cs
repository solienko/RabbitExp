using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PublisherWeb.Models;

namespace PublisherWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventBus _eventBus;

        public HomeController(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            ViewData["Message"] = "Test page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Zero(Inner inner)
        {
            _eventBus.Publish(new MyMessage0 { InnerProp = inner });
            return RedirectToAction("Test");
        }

    }
}
