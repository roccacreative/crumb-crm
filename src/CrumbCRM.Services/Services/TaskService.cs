using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data;

namespace CrumbCRM.Services
{
    public class TaskService : ServiceBase<ITaskRepository>, ITaskService
    {
        public int Save(Task task)
        {
            return Repository.Save(task);
        }

        public Task GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Task> GetAll(Filters.TaskFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Delete(Task task) 
        {
            return Repository.Delete(task);
        }

        public bool Edit(Task task) 
        {
            return Repository.Edit(task);
        }
        public int Total(Filters.TaskFilterOptions options)
        {
            return Repository.Total(options);
        }
    }
}
