using System;
using System.Collections.Generic;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;
using System.Linq;

namespace TodoList.DataAccess.DTO
{
    public class EntryDto
    {
        /// Правила ролевого доступа
        public List<ProblemStatus> ValidStatuses { get; set; }

        public List<ActionStatus> ValidActions { get; set; }

        /// Поля EntryEntity
        public int EntryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public UserEntity Initiator { get; set; }

        public UserEntity Executor { get; set; }

        // av: "Expiration"
        public DateTime Deadline { get; set; }

        public string Report { get; set; }

        public ProblemStatus TaskStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public EntryDto(EntryEntity entry)
        {
            EntryId = entry.EntryId;
            Title = entry.Title;
            Description = entry.Description;
            Initiator = entry.Initiator;
            Executor = entry.Executor;
            Deadline = entry.Deadline;
            Report = entry.Report;
            TaskStatus = entry.TaskStatus;
            StartDate = entry.StartDate;
            CompletionDate = entry.CompletionDate;
        }

        public static List<EntryDto> ConvertToDto(List<EntryEntity> entries)
        {
            return entries
                .Select(e => new EntryDto(e))
                .ToList();
        }
    }
}