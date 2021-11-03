using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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

        public DateTime Deadline { get; set; }

        public string Report { get; set; }

        public ProblemStatusEntity TaskStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int CurrentPage { get; set; }

        // NB: необходим для десериализации
        [JsonConstructor]
        public EntryDto() { }

        public EntryDto(EntryEntity entry, int currentPage = 0)
        {
            if (entry == null) return;
            
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
            CurrentPage = currentPage;
        }

        public EntryDto(String message)
        {
            Description = message;
        }

        internal static EntryEntity ConvertFromDto(EntryDto model)
        {
            // TODO: убери костыль
            EntryEntity entryEntity = new()
            {
                CompletionDate = model.CompletionDate,
                Deadline = model.Deadline,
                Description = model.Description,
                EntryId = model.EntryId,
                Executor = model.Executor,
                Initiator = model.Initiator,
                Report = "костыль", // TODO: model.Report,
                StartDate = model.StartDate,
                TaskStatus = model.TaskStatus,
                Title = model.Title
            };
            return entryEntity;
        }

        public static List<EntryDto> ConvertToDto(List<EntryEntity> entries, int currentPage = 0)
        {
            return entries
                .Select(e => new EntryDto(e, currentPage))
                .ToList();
        }

        public static EntryDto ConvertToDto(EntryEntity entry, int currentPage = 0)
        {
            return new EntryDto(entry, currentPage);
        }

        public static List<EntryDto> Error(String message)
        {
            return new List<EntryDto> { new (message) };
        }
    }
}