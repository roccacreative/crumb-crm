using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Services;
using CrumbCRM.Data.Entity.Extensions;
using CrumbCRM.Filters;

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
            var leads = Context.Lead
                .Include("Company")
                .Include("OwnerUser")
                .Include("Tags")
                .Include("Tags.Tag")
                .Where(q => !q.Deleted.HasValue && !q.Converted.HasValue);

            if (options != null)
            {
                if (options.Type.HasValue)
                    leads = leads.Where(c => c.Status == options.Type.Value);

                if (options.Order == "desc")
                {
                    leads = leads.OrderByDescending(x => x.CreatedDate);
                }

                if (options.Campaigns != null)
                {
                    var ids = options.Campaigns.Select(c => ((Campaign)c).ID).ToList();
                    leads = leads.Where(l => l.CampaignID.HasValue && ids.Contains(l.CampaignID.Value));
                }

                if (options.Tags != null)
                {
                    var ids = options.Tags.Select(c => ((Tag)c).ID);
                    leads = leads.Where(l => l.Tags.Where(t => ids.Contains(t.TagID)).Count() > 0);
                }

                if (options.StartDate.HasValue)
                    leads = leads.Where(x => x.CreatedDate > options.StartDate.Value);

                if (options.EndDate.HasValue)
                    leads = leads.Where(x => x.CreatedDate < options.EndDate.Value);

                if (!string.IsNullOrEmpty(options.SearchTerm))
                {
                    leads = leads.Where(x => (!string.IsNullOrEmpty(x.FirstName) && x.FirstName.Contains(options.SearchTerm)) ||
                                                    (!string.IsNullOrEmpty(x.LastName) && x.LastName.Contains(options.SearchTerm)) ||
                                                    (!string.IsNullOrEmpty(x.JobTitle) && x.JobTitle.Contains(options.SearchTerm)));
                }
            }

            if (paging != null)
            {
                leads = leads.Distinct().OrderByDescending(l => l.CreatedDate).ToPagedQueryable(paging);
            }

            return leads;
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
