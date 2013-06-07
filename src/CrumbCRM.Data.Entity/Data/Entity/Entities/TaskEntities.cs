using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Services;
using CrumbCRM.Data.Entity.Extensions;

namespace CrumbCRM.Data.Entity.Entities
{
    public class TaskEntities : EntitiesBase<CrumbCRMEntities>, ITaskRepository
    {
        public int Save(Task task)
        {
            if (task.ID == 0)
            {
                Context.Task.Add(task);
            }
            else
            {
                Context.Task.Attach(task);
                Context.Entry(task).State = System.Data.Entity.EntityState.Modified;
            }


            Context.SaveChanges();
            return task.ID;
        }

        public Task GetByID(int id)
        {
            return Context.Task
                .Include("AssignedUser")
                .FirstOrDefault(i => i.ID == id);
        }

        public List<Task> GetAll(Filters.TaskFilterOptions options = null, PagingSettings paging = null)
        {
            var tasks = QueryTasks(options, paging);

            return tasks.ToList();
        }

        private IQueryable<Task> QueryTasks(Filters.TaskFilterOptions options, PagingSettings paging = null)
        {
            var tasks = Context.Task
                .Include("AssignedUser")
                .Where(q => !q.Deleted.HasValue);


            if (options != null)
            {
                if (options.Order == "desc")
                {
                    tasks = tasks.OrderByDescending(x => x.CreatedDate);
                }

                if (options.HasDeadline.HasValue)
                    tasks = tasks.Where(t => t.DueDate.HasValue == options.HasDeadline.Value);

                if (options.FutureOnly.HasValue)
                {
                    if (options.FutureOnly.Value)
                        tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value >= DateTime.Now);
                    else
                        tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value <= DateTime.Now);
                }

                if (options.StartDate.HasValue)
                    tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value >= options.StartDate.Value);

                if (options.EndDate.HasValue)
                    tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value <= options.EndDate.Value);
            }
            if (paging != null)
            {
                tasks = tasks.Distinct().OrderByDescending(l => l.CreatedDate).ToPagedQueryable(paging);
            }
            return tasks;
        }

        public bool Delete(Task task) {

            if (task != null)
            {
                task.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Task task) 
        {
            Context.SaveChanges();
            return true;
        }

        public int Total(Filters.TaskFilterOptions options)
        {
            return QueryTasks(options).Count();
        }
    }
}
