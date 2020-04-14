using System;
using System.Collections.Generic;

namespace TodoProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
