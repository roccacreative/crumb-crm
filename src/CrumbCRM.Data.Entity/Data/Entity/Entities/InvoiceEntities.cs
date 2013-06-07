using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Services;
using CrumbCRM.Filters;
using CrumbCRM.Data.Entity.Extensions;

namespace CrumbCRM.Data.Entity.Entities
{
    public class InvoiceEntities : EntitiesBase<CrumbCRMEntities>, IInvoiceRepository
    {
        public int Save(Invoice invoice)
        {
            if (invoice.ID == 0)
                Context.Invoice.Add(invoice);

            Context.SaveChanges();
            return invoice.ID;
        }

        public Invoice GetByID(int id)
        {
            return Context.Invoice.FirstOrDefault(i => i.ID == id);
        }

        public List<Invoice> GetAll(Filters.InvoiceFilterOptions options = null, PagingSettings paging = null)
        {
            var invoices = QueryInvoices(options, paging);
            return invoices.ToList();
        }

        private IQueryable<Invoice> QueryInvoices(Filters.InvoiceFilterOptions options, PagingSettings paging = null)
        {
            var invoices = Context.Invoice.Where(i => !i.Deleted.HasValue);

            if(options != null) {
                if (options.Order == "desc")
                {
                    invoices = invoices.OrderByDescending(x => x.CreatedDate);
                }
            }

            if (paging != null)
            {
               invoices = invoices.Distinct().OrderByDescending(l => l.CreatedDate).ToPagedQueryable(paging);
            }
            return invoices;
        }

        public bool Void()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Invoice invoice)
        {
            if (invoice != null)
            {
                invoice.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Invoice invoice)
        {
            Context.SaveChanges();
            return true;
        }


        public bool EditItem(InvoiceItem invoiceItem)
        {
            Context.SaveChanges();
            return true;
        }

        public bool DeleteItem(InvoiceItem invoiceItem)
        {
            if (invoiceItem != null)
            {
                invoiceItem.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<InvoiceItem> GetAllItems(InvoiceItemFilterOptions options = null, PagingSettings paging = null)
        {
            return Context.InvoiceItem.Where(q => !q.Deleted.HasValue).ToList();
        }

        public InvoiceItem GetItemByID(int id)
        {
            return Context.InvoiceItem.FirstOrDefault(i => i.ID == id);
        }

        public List<InvoiceItem> GetItemsByInvoiceID(int id)
        {
            return Context.InvoiceItem.Where(i => i.InvoiceID == id && !i.Deleted.HasValue).ToList();
        }

        public int SaveItem(InvoiceItem invoiceItem)
        {
            if (invoiceItem.ID == 0)
            {
                Context.InvoiceItem.Add(invoiceItem);
            }
            else
            {
                Context.InvoiceItem.Attach(invoiceItem);
                Context.Entry(invoiceItem).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return invoiceItem.ID;
        }


        public int Total(Filters.InvoiceFilterOptions options)
        {
            return QueryInvoices(options).Count();
        }
    }
}
