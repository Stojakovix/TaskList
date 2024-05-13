using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Model
{
    public class TaskItem
    {
        [PrimaryKey , AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? urgency { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
