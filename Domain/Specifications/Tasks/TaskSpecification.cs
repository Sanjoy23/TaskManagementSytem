using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.Tasks
{
    public class TaskSpecification : BaseSpecification<TaskEntity>
    {
        public TaskSpecification(TaskSpecParams taskParams)
            : base(x => (string.IsNullOrEmpty(taskParams.Search) || x.Title.ToLower().Contains(taskParams.Search)))
        {
            AddInclude(x => x.Status);
            AddInclude(x => x.Team);
            AddInclude(x => x.AssignedToUser);
            AddInclude(x => x.CreatedByUser);
            AddOrderBy(x => x.Title);

            ApplyPaging(taskParams.PageSize * (taskParams.PageIndex - 1), taskParams.PageSize);

            if (!string.IsNullOrEmpty(taskParams.Sort))
            {
                switch (taskParams.Sort)
                {
                    case "titleAsc":
                        AddOrderBy(x => x.Title);
                        break;
                    case "titleDesc":
                        AddOrderByDescending(x => x.Title);
                        break;
                    case "dueDateAsc":
                        AddOrderBy(x => x.DueDate);
                        break;
                    case "dueDateDesc":
                        AddOrderByDescending(x => x.DueDate);
                        break;
                    default:
                        AddOrderBy(x => x.Title);
                        break;
                }
            }
        }
    }
}
