using BulkeyWebRazor_Temp.Data;
using BulkeyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkeyWebRazor_Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        //display all categories
        private readonly ApplicationDbContext _db;
        public List<Category> CategoryList { get; set; }
        //creating dependency injection
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
