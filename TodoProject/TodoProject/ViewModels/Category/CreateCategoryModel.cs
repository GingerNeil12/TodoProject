﻿using System.ComponentModel.DataAnnotations;

namespace TodoProject.ViewModels.Category
{
    public class CreateCategoryModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 100, ErrorMessage = "Cannot exceed {0} characters.")]
        public string Name { get; set; }
    }
}
