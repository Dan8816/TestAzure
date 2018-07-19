using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Models
{
    public class CustomDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date > DateTime.Now;
        }
    }
    public class NewProduct
    {
        [Key]
        public int Id { get; set; }//Primary Key
        [Required(ErrorMessage = "Product Name cannot be blank")]
        [MinLength(2,ErrorMessage = "Min 2 characters.")]
        [MaxLength(45, ErrorMessage = "Max 45 characters.")]
        public string ProductName {get;set;}//Must match schema name
        [Required(ErrorMessage = "Product Desciption cannot be blank")]

        [MinLength(2,ErrorMessage = "Min 2 characters.")]
        [MaxLength(45, ErrorMessage = "Max 45 characters.")]
        public string Description {get;set;}//must match the schema name

        [Required(ErrorMessage = "Price cannot be blank")]
        [Range(0,1000)]
        public double Price {get;set;}//must match schema

        public DateTime Created_At { get; set; }//this must match the table field name and case wise
        public DateTime Updated_At { get; set; }//this must match the table field name and case wise
    }
    public class NewCategory
    {
        [Key]
        public int Id { get; set; }//Primary Key
        [Required(ErrorMessage = "Category Name cannot be blank")]
        [MinLength(2,ErrorMessage = "Min 2 characters.")]
        [MaxLength(45, ErrorMessage = "Max 45 characters.")]
        public string CategoryName {get;set;}//Must match schema name
        public DateTime Created_At { get; set; }//this must match the table field name and case wise
        public DateTime Updated_At { get; set; }//this must match the table field name and case wise 

    }
    public class ViewModel
    {
        public Product ViewProduct {get;set;}
        public Category ViewCategory {get;set;}
        public Groupings Groupings {get;set;}
        public List<Product> ProductList {get;set;}
        public List<Category> CategoryList {get;set;}
    }
}