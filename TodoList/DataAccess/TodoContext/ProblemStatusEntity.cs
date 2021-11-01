namespace TodoList.DataAccess.TodoContext
{
    public class ProblemStatusEntity : IEntity
    {
        public int ProblemStatusId { get; set; }

        //public ProblemStatus ProblemStatus { get; set; }

        public string ProblemStatusName { get; set; }
    }
}