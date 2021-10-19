using System;
using System.Threading.Tasks;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess.DTO
{
    public class EntryDto
    {
        public int EntryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public UserEntity Initiator { get; set; }

        public UserEntity Executor { get; set; }

        // av: "Expiration"
        public DateTime Deadline { get; set; }

        public string Report { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }
    }
}