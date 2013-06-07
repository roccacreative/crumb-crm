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

        public List<Campaign> GetAll()
        {
            return Context.Campaign.Where(c => !c.Deleted.HasValue).ToList();
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
