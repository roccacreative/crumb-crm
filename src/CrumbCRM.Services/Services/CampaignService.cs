using CrumbCRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services
{
    public class CampaignService : ServiceBase<ICampaignRepository>, ICampaignService
    {
        public Campaign GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Campaign> GetAll()
        {
            return Repository.GetAll();
        }

        public int Save(Campaign campaign)
        {
            return Repository.Save(campaign);
        }
    }
}
