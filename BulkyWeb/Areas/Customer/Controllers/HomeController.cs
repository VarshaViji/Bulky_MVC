using BulkyWeb.Models;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;

namespace BulkyWeb.Areas.Customer.Controllers
{
    //defining controller to specific area
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        //getting product list to display it in home page
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        //to get details of product when click on detail button
        public IActionResult Details(int productId)
        {
            Product product= _unitOfWork.Product.Get(u=>u.Id==productId,includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
