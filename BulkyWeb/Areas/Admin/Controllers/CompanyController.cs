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
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        //1.get all categories
        private readonly IUnitOfWork _unitOfWork;
        //to access wwwroot folder for image
        private readonly IWebHostEnvironment _webHostEnvironment;
        //dependency injection
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //2.creating list to get all categories
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            //retrieving category list to pass it to create dropdown for category
            //converting category to IEnumerable selectlistitem using projection
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //     .GetAll().Select(u => new SelectListItem  //Projection in ef core
            //     {
            //         Text = u.Name,
            //         Value = u.Id.ToString()
            //     });
                       
            //passing obj to the view
            return View(objCompanyList);
        }
        //create a create action method for create company ui
        //changing create to upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = 
            //adding viewbag
            //ViewBag.CategoryList = CategoryList;
            //adding viewdata
           //ViewData["CategoryList"] = CategoryList;
            //passing CompanyVM
  
            if(id==null || id==0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }
        //Create company
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            //for adding new company & redirection

            //implementing Server side validation
            if (ModelState.IsValid)
            {
                //to access wwwroot
                
                //to save the file to wwwrooot folder
               

                //checking for update or create
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                //Creating TempData
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
               
                return View(CompanyObj);
            }
        }
        //create action method for Edit button
       
        //Create action method for Delete
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        //for delete button using datatable
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successful" });
        }
        #endregion
    }
}
