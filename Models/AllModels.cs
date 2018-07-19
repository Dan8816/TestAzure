using System;
using System.Collections.Generic;//allows creating of list objects
using System.ComponentModel.DataAnnotations;//dependency for validations compared to models
using System.ComponentModel.DataAnnotations.Schema;//dependency for validations compared to db schema

namespace Products.Models
{
    public class Product
    {
        [Key]
        public int Id {get;set;}//primary key
        public string ProductName {get;set;}//must match the schema name
        public string Description {get;set;}//must match the schema name
        public double Price {get;set;}//must match the schema name
        public DateTime Created_At { get; set; }//must match the schema name
        public DateTime Updated_At { get; set; }//must match the schema name
        public List<Groupings> Grouplist {get;set;}//this instantiates a list of Categorized objects from the categorized table connected to a Product object for M2M
        public Product()
        {
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
            Grouplist = new List<Groupings>();        
        }
    }
    public class Category
    {
        [Key]
        public int Id {get;set;}//primary key
        public string CategoryName {get;set;}//must match the schema name
        public DateTime Created_At { get; set; }//must match the schema name
        public DateTime Updated_At { get; set; }//must match the schema name
        public List<Groupings> Grouplist {get;set;}//this instantiates a list of Categorized objects from the categorized table connected to a Product object for M2M
        public Category()
        {
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
            Grouplist = new List<Groupings>();        
        }
    }

    [Table("categorized")]  
    public class Groupings
    {
        [Key]
        public int Id {get;set;}//primary key
        public int ProductId {get;set;}//must match the schema name
        public Product CategorizedProduct {get;set;}//this will refer to an instance of a Product attached to the foreign key ProductId
        public int CategoryId {get;set;}
        public Category AddedCategory {get;set;}//this will refer to an instance of a Category attached to the foreign key CategoryId
    }
}