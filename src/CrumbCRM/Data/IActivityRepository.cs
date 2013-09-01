using CrumbCRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data
{
    public interface IActivityRepository : IDisposable
    {
        Activity GetByID(int id);
        List<Activity> GetAll(PagingSettings paging);
        List<Activity> GetByType(AreaType type);
        int Save(Activity activity);
    }
}
