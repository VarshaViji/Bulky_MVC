using Bulky.Utility;
using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Models.ViewModels;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.ObjectPool;

namespace BulkyWeb.Areas.Admin.Controllers
{
    //defining controller to specific area
    [Area("Admin")]
    //adding authorization to visible only to admin
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        //1.get all categories
        private readonly IUnitOfWork _unitOfWork;
        //to access wwwroot folder for image
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //2.creating list to get all categories
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            //retrieving category list to pass it to create dropdown for category
            //converting category to IEnumerable selectlistitem using projection
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //     .GetAll().Select(u => new SelectListItem  //Projection in ef core
            //     {
            //         Text = u.Name,
            //         Value = u.Id.ToString()
            //     });
                       
            //passing obj to the view
            return View(objProductList);
        }
        //create a create action method for create product ui
        //changing create to upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = 
            //adding viewbag
            //ViewBag.CategoryList = CategoryList;
            //adding viewdata
           //ViewData["CategoryList"] = CategoryList;
            //passing ProductVM
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem  //Projection in ef core
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 }),
                Product = new Product()
            };
            if(id==null || id==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        //Create product
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            //for adding new product & redirection

            //implementing Server side validation
            if (ModelState.IsValid)
            {
                //to access wwwroot
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //to save the file to wwwrooot folder
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    //to update the existing image in imageurl
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //uploading images
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                //checking for update or create
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                //Creating TempData
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem  //Projection in ef core
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }
        //create action method for Edit button
       
        //Create action method for Delete
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        //for delete button using datatable
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                            Path.Combine(_webHostEnvironment.WebRootPath,
                            productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successful" });
        }
        #endregion
    }
}
