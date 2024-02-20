using Azure.Core;
using BulkeyWebRazor_Temp.Data;
using BulkeyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkeyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category{ get; set; }
        //creating dependency injection
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }

        // code to add a new category to category table
        public IActionResult OnPost()
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            //adding tempdata for toastr notification
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
    }
}
