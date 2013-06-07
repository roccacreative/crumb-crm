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
    public class LeadEntities : EntitiesBase<CrumbCRMEntities>, ILeadRepository
    {
        public int Save(Lead lead)
        {
            if (lead.ID == 0)
            {
                Context.Lead.Add(lead);
            }
            else
            {
                Context.Lead.Attach(lead);
                Context.Entry(lead).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return lead.ID;
        }

        public Lead GetByID(int id)
        {
            var lead = Context.Lead
                .Include("Company")
                .Include("OwnerUser")
                .Include("Tags")
                .Include("Tags.Tag")
                .FirstOrDefault(i => i.ID == id);

            lead.LastNote = Context.Note.Where(n => n.ItemID == id && n.Type == NoteType.Lead).OrderByDescending(n => n.CreatedDate).FirstOrDefault();

            return lead;
        }



        public List<Lead> GetAll(Filters.LeadFilterOptions options = null, PagingSettings paging = null)
        {
            var Leads = QueryLeads(options, paging);

            Leads.ToList().ForEach(l => l.LastNote = Context.Note.Where(n => n.ItemID == l.ID && n.Type == NoteType.Lead).OrderByDescending(n => n.CreatedDate).FirstOrDefault());

            return Leads.ToList();
        }

        private IQueryable<Lead> QueryLeads(Filters.LeadFilterOptions options, PagingSettings paging = null)
        {
            var Leads = Context.Lead
                .Include("Company")
                .Include("OwnerUser")
                .Include("Tags")
                .Include("Tags.Tag")
                .Where(q => !q.Deleted.HasValue && !q.Converted.HasValue);

            if (options != null)
            {
                if (options.Status.HasValue)
                    Leads = Leads.Where(c => c.Status == options.Status.Value);

                if (options.Order == "desc")
                {
                    Leads = Leads.OrderByDescending(x => x.CreatedDate);
                }

                if (options.Campaigns != null)
                {
                    var ids = options.Campaigns.Select(c => ((Campaign)c).ID).ToList();
                    Leads = Leads.Where(l => l.CampaignID.HasValue && ids.Contains(l.CampaignID.Value));
                }

                if (options.Tags != null)
                {
                    var ids = options.Tags.Select(c => ((Tag)c).ID);
                    Leads = Leads.Where(l => l.Tags.Where(t => ids.Contains(t.TagID)).Count() > 0);
                }
            }

            if (paging != null)
            {
                Leads = Leads.Distinct().OrderByDescending(l=>l.CreatedDate).ToPagedQueryable(paging);
            }

            return Leads;
        }

        public bool Delete(Lead Lead) {

            if (Lead != null)
            {
                Lead.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Lead Lead) 
        {
            Context.SaveChanges();
            return true;
        }

        public bool AssignTag(Lead lead, Tag tag)
        {
            Context.LeadTag.Add(new LeadTag()
            {
                LeadID = lead.ID,
                TagID = tag.ID
            });

            Context.SaveChanges();
            return true;
        }


        public int Total(Filters.LeadFilterOptions options)
        {
            return QueryLeads(options).Count();
        }
    }
}
