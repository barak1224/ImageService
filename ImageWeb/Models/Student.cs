using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageWeb.Models
{
    public class Student
    {
        public Student()
        {
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
    }
}