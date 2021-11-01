using System;

namespace TodoList.DataAccess.TodoContext
{
    public class EntryEntity : IEntity
    {
        public int EntryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        // будет добавлен FK: InitiatorUserId
        public UserEntity Initiator { get; set; }

        // будет добавлен FK: ExecutorUserId
        public UserEntity Executor { get; set; }

        // av: "Expiration"
        public DateTime Deadline { get; set; }

        public string Report { get; set; }

        public ProblemStatusEntity TaskStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        // Many-to-many на данный момент не нужно, но понадобится в дальнейшем
        //public ICollection<UserEntryEntity> UserEntryBind { get; set; }
    }

    //    NB: поле типа public int InitiatorId { get; set; } вызывает ошибку: 
    //
    //    Microsoft.Data.SqlClient.SqlException:
    //    'Introducing FOREIGN KEY constraint 'FK_Entries_Users_InitiatorId' on table 'Entries'
    //    may cause cycles or multiple cascade paths.
    //    Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
    //    Could not create constraint or index.See previous errors.'
    //
    //    если необходимо имя с окончанием на Id выбирай тип long
}