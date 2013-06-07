using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Filters;

namespace CrumbCRM.Data
{
    public interface IInvoiceRepository : IRepositoryBase
    {
        int Save(Invoice invoice);
        Invoice GetByID(int id);
        List<Invoice> GetAll(InvoiceFilterOptions options = null, PagingSettings paging = null);
        bool Void();
        bool Delete(Invoice invoice);
        bool Edit(Invoice invoice);

        int SaveItem(InvoiceItem invoiceItem);
        InvoiceItem GetItemByID(int id);
        List<InvoiceItem> GetItemsByInvoiceID(int id);
        List<InvoiceItem> GetAllItems(InvoiceItemFilterOptions options = null, PagingSettings paging = null);
        bool DeleteItem(InvoiceItem invoiceItem);
        bool EditItem(InvoiceItem invoiceItem);

        int Total(InvoiceFilterOptions options);
    }
}
