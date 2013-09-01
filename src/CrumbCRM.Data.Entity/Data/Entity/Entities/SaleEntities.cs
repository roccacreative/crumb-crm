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
    public class SaleEntities : EntitiesBase<CrumbCRMEntities>, ISaleRepository
    {
        public int Save(Sale sale)
        {
            if (sale.ID == 0)
            {
                Context.Sale.Add(sale);
            }
            else
            {
                Context.Sale.Attach(sale);
                Context.Entry(sale).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return sale.ID;
        }

        public Sale GetByID(int id)
        {
            var sale = Load(Context.Sale
                .Include("OwnerUser")
                .Include("Tags")
                .Include("Tags.Tag")
                .Include("Person")
                .FirstOrDefault(i => i.ID == id));

            sale.LastNote = Context.Note.Where(n => n.ItemID == id && n.Type == NoteType.Lead).OrderByDescending(n => n.CreatedDate).FirstOrDefault();
            return sale;
        }

        private Sale Load(Sale sale)
        {
            sale.Company = new ContactEntities().GetByID(sale.CompanyID);
            sale.LastNote = Context.Note.Where(n => n.ItemID == n.ID && n.Type == NoteType.Lead).OrderByDescending(n => n.CreatedDate).FirstOrDefault();

            return sale;
        }

        public List<Sale> GetAll(Filters.SaleFilterOptions options = null, PagingSettings paging = null)
        {
            var sales = QuerySales(options, paging);
            return Load(sales.ToList());
        }

        private IQueryable<Sale> QuerySales(Filters.SaleFilterOptions options, PagingSettings paging = null)
        {
            var sales = Context.Sale
                .Include("OwnerUser")
                .Include("Tags")
                .Include("Tags.Tag")
                .Include("Person")
                .Where(q => !q.Deleted.HasValue);

            if (options != null)
            {
                if (options.Status.HasValue)
                    sales = sales.Where(c => c.Status == options.Status.Value);

                if (options.Order == "desc")
                {
                    sales = sales.OrderByDescending(x => x.CreatedDate);
                }

                if (options.Campaigns != null)
                {
                    var ids = options.Campaigns.Select(c => ((Campaign)c).ID).ToList();
                    sales = sales.Where(l => l.CampaignID.HasValue && ids.Contains(l.CampaignID.Value));
                }

                if (options.Tags != null)
                {
                    var ids = options.Tags.Select(c => ((Tag)c).ID);
                    sales = sales.Where(l => l.Tags.Where(t => ids.Contains(t.TagID)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(options.SearchTerm))
                {
                    sales = sales.Where(x => (!string.IsNullOrEmpty(x.Person.FirstName) && x.Person.FirstName.Contains(options.SearchTerm)) ||
                                                    (!string.IsNullOrEmpty(x.Person.LastName) && x.Person.LastName.Contains(options.SearchTerm)) ||
                                                    (!string.IsNullOrEmpty(x.JobTitle) && x.JobTitle.Contains(options.SearchTerm)));
                }
            }

            if (paging != null)
            {
                sales = sales.Distinct().OrderByDescending(l => l.CreatedDate).ToPagedQueryable(paging);
            }

            return sales;
        }

        private List<Sale> Load(List<Sale> sales)
        {
            sales.ForEach(s => Load(s));
            return sales;
        }

        public bool Delete(Sale sale)
        {
            if (sale != null)
            {
                sale.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Sale Lead) 
        {
            Context.SaveChanges();
            return true;
        }

        public bool AssignTag(Sale sale, Tag tag)
        {
            Context.SaleTag.Add(new SaleTag()
            {
                SaleID = sale.ID,
                TagID = tag.ID
            });

            Context.SaveChanges();
            return true;
        }

        public int Total(Filters.SaleFilterOptions options)
        {
            return QuerySales(options).Count();
        }


        public decimal Sum(Filters.SaleFilterOptions options)
        {
            return QuerySales(options).Sum(s => (decimal?)s.Value) ?? 0;
        }
    }
}
