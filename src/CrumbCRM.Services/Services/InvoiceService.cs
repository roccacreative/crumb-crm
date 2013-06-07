using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data;

namespace CrumbCRM.Services
{
    public class InvoiceService : ServiceBase<IInvoiceRepository>, IInvoiceService
    {
        public int Save(Invoice invoice)
        {
            return Repository.Save(invoice);
        }

        public Invoice GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Invoice> GetAll(Filters.InvoiceFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Void()
        {
            return Repository.Void();
        }

        public bool Delete(Invoice invoice)
        {
            return Repository.Delete(invoice);

        }

        public bool Edit(Invoice invoice)
        {
            return Repository.Edit(invoice);
        }


        // items

        public int SaveItem(InvoiceItem invoiceItem)
        {
            return Repository.SaveItem(invoiceItem);
        }

        public bool DeleteItem(InvoiceItem invoiceItem)
        {
            return Repository.DeleteItem(invoiceItem);
        }

        public InvoiceItem GetItemByID(int id)
        {
            return Repository.GetItemByID(id);
        }
        public List<InvoiceItem> GetItemsByInvoiceID(int id)
        {
            return Repository.GetItemsByInvoiceID(id);
        }

        public List<InvoiceItem> GetAllItems(Filters.InvoiceItemFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAllItems(options, paging);
        }

        public bool EditItem(InvoiceItem invoiceItem)
        {
            return Repository.EditItem(invoiceItem);
        }

        public int Total(Filters.InvoiceFilterOptions options)
        {
            return Repository.Total(options);
        }
    }
}
