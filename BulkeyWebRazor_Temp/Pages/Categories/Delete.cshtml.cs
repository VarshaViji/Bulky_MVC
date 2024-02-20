using BulkeyWebRazor_Temp.Data;
using BulkeyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkeyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        //creating dependency injection
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category = _db.Categories.Find(id);
            }
        }

        // code to delete a category from category table
        public IActionResult OnPost()
        {
            //for delete category & redirection
            //need to retrieve data from catogory and then we will delete it
            Category? obj = _db.Categories.Find(Category.Id);

            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            //adding tempdata for toastr notification
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}