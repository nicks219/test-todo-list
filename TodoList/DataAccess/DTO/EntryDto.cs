using System;
using System.Collections.Generic;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;
using System.Linq;
using System.Text.Json.Serialization;

namespace TodoList.DataAccess.DTO
{
    public class EntryDto
    {
        /// Правила ролевого доступа
        public List<ProblemStatus> ValidStatuses { get; set; }

        public List<ActionStatus> ValidActions { get; set; }

        /// Поля EntryEntity
        //[JsonPropertyName("entryId")]
        public int EntryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public UserEntity Initiator { get; set; }

        public UserEntity Executor { get; set; }

        // av: "Expiration"
        public DateTime Deadline { get; set; }

        public string Report { get; set; }

        public ProblemStatusEntity TaskStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int CurrentPage { get; set; }

        public EntryDto(EntryEntity entry, int currentPage = 0)
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
            CurrentPage = currentPage;
        }

        [JsonConstructor]
        public EntryDto()
        {
            Console.Write("!");
        }

        public EntryDto(String message)
        {
            Title = message;
        }

        internal static EntryEntity ConvertFromDto(EntryDto model)
        {
            EntryEntity entryEntity = new EntryEntity();

            var a = "!";
            entryEntity.CompletionDate = model.CompletionDate;
            entryEntity.Deadline = model.Deadline;
            entryEntity.Description = model.Description + a;
            entryEntity.EntryId = model.EntryId;
            entryEntity.Executor = model.Executor;
            entryEntity.Initiator = model.Initiator;
            entryEntity.Report = model.Report + a;
            entryEntity.StartDate = model.StartDate;
            entryEntity.TaskStatus = model.TaskStatus;
            entryEntity.Title = model.Title + a;
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
            return new List<EntryDto> { new EntryDto(message) };
        }
    }
}