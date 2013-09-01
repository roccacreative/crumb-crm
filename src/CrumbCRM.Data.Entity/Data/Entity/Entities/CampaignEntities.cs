using CreativeCRM.Filters;
using CrumbCRM.Data.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data.Entity.Entities
{
    public class CampaignEntities : EntitiesBase<CrumbCRMEntities>, ICampaignRepository
    {
        public Campaign GetByID(int id)
        {
            return Context.Campaign.FirstOrDefault(c => c.ID == id);
        }

        public List<Campaign> GetAll(CampaignFilterOptions options = null)
        {
            var query = Context.Campaign.Where(c => !c.Deleted.HasValue);

            if (options != null)
            {
                if(!string.IsNullOrEmpty(options.SearchTerm))
                {
                    query = query.Where(x => (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(options.SearchTerm)) ||
                        (!string.IsNullOrEmpty(x.Description) && x.Description.Contains(options.SearchTerm)));
                }
            }

            return query.ToList();
        }

        public int Save(Campaign campaign)
        {
            if (campaign.ID == 0)
            {
                Context.Campaign.Add(campaign);
            }
            else
            {
                Context.Campaign.Attach(campaign);
                Context.Entry(campaign).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return campaign.ID;
        }
    }
}
