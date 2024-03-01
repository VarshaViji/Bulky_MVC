using Bulky.Utility;
using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BulkyWeb.Areas.Admin.Controllers
{
    //defining controller to specific area
    [Area("Admin")]
    //adding authorization to visible only to admin
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        //1.get all categories
        public readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //2.creating list to get all categories
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            //passing obj to the view
            return View(objCategoryList);
        }
        //create a create action method for create category ui
        public IActionResult Create()
        {
            return View();
        }
        //Create category
        [HttpPost]
        public IActionResult Create(Category obj)
        {

            //implementing custom validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            }

            //for adding new category & redirection

            //implementing Server side validation
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                //Creating TempData
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //create action method for Edit button
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //Create post for edit action method
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //for updating category & redirection

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //Create action method for Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //Create post for delete action method
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            //for delete category & redirection
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
