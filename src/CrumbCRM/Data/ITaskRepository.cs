using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Filters;

namespace CrumbCRM.Data
{
    public interface ITaskRepository : IRepositoryBase
    {
        int Save(Task task);
        Task GetByID(int id);
        List<Task> GetAll(TaskFilterOptions options = null, PagingSettings paging = null);
        bool Delete(Task task);
        bool Edit(Task task);

        int Total(TaskFilterOptions options);
    }
}
