using System;
using System.Collections.Generic;

namespace TodoProject.Models
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasStarted { get; set; }
        public DateTime StartedOn { get; set; }
        public bool HasCompleted { get; set; }
        public DateTime CompletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
