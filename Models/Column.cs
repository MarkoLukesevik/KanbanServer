﻿namespace KanbanApp.Models
{
    public class Column
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }
}