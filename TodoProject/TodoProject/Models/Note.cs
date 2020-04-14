using System;

namespace TodoProject.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool CreatedAutomatically { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TodoItemId { get; set; }

        public virtual TodoItem TodoItem { get; set; }
    }
}
