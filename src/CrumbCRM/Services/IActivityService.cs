using CrumbCRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services
{
    public interface IActivityService
    {
        Activity GetByID(int id);
        List<Activity> GetAll();
        List<Activity> GetByType(AreaType type);
        int Save(Activity activity);
        int Create(string description, AreaType type, string userName = null, int? itemId = null); 
    }
}
