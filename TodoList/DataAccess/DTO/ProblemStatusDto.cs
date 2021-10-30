using System.Collections.Generic;
using System.Linq;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess.DTO
{
    public class ProblemStatusDto
    {
        public int ProblemStatusId { get; set; }

        public string ProblemStatusName { get; set; }

        public ProblemStatusDto(ProblemStatusEntity problemStatus)
        {
            ProblemStatusId = problemStatus.ProblemStatusId;
            ProblemStatusName = problemStatus.ProblemStatusName;
        }

        public static List<ProblemStatusDto> ConvertToDto (List<ProblemStatusEntity> problemStatusEntities)
        {
            return problemStatusEntities
                .Select(p => new ProblemStatusDto(p))
                .ToList();
        }
    }
}
