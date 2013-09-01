using CreativeCRM.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data
{
    public interface ICampaignRepository : IDisposable
    {
        Campaign GetByID(int id);
        List<Campaign> GetAll(CampaignFilterOptions options = null);
        int Save(Campaign campaign);
    }
}
