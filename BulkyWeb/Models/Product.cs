using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        //listprice is a different price
        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double  ListPrice{get;set;}
        //cost of product for quantity 1 to 50
        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }
        //cost of product for quantity more than 50
        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }
        //cost of product for quantity more than 100
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }
        //creating navigation to category table using foreign key constraint
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        //[ValidateNever]
        //public string ImageUrl { get; set; }       //removing image url for mutiple picture of products

        //creating list for product images for multiple images
        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
    }
}
