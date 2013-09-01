using CrumbCRM.Data;
using CrumbCRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services
{
    public class ActivityService : ServiceBase<IActivityRepository>, IActivityService
    {
        private readonly IMembershipService _membershipService;

        public ActivityService(
            IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public Activity GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Activity> GetAll(PagingSettings paging = null)
        {
            return Repository.GetAll(paging);
        }

        public List<Activity> GetByType(AreaType type)
        {
            return Repository.GetByType(type);
        }

        public int Save(Activity activity)
        {
            return Repository.Save(activity);
        }

        public int Create(string description, AreaType type, string userName = null, int? itemId = null)
        {
            int userId = _membershipService.GetByUsername(userName).UserId;

            return Save(new Activity()
            {
                Description = description,
                ItemID = itemId,
                UserID = userId,
                Type = type
            });
        }
    }
}
