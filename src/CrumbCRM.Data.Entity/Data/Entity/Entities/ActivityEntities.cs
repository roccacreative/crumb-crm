using CrumbCRM.Data;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data.Entity.Entities
{
    public class ActivityEntities : EntitiesBase<CrumbCRMEntities>, IActivityRepository
    {
        public Activity GetByID(int id)
        {
            return Context.Activity.FirstOrDefault(a => a.ID == id);
        }

        public List<Activity> GetAll()
        {
            return Context.Activity.Include("User").OrderByDescending(a => a.ActivityDate).ToList();
        }

        public List<Activity> GetByType(AreaType type)
        {
            return Context.Activity.Where(t => t.Type == type).ToList();
        }

        public int Save(Activity activity)
        {
            if (activity.ID == 0)
            {
                Context.Activity.Add(activity);
            }
            else
            {
                Context.Activity.Attach(activity);
                Context.Entry(activity).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return activity.ID;
        }
    }
}
