using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Filters;

namespace CrumbCRM.Data
{
    public interface IQuoteRepository : IRepositoryBase
    {
        int Save(Quote quote);
        Quote GetByID(int id);
        List<Quote> GetAll(QuoteFilterOptions options = null, PagingSettings paging = null);
        bool Void();
        bool Delete(Quote quote);
        bool Edit(Quote quote);

        int SaveItem(QuoteItem quoteItem);
        QuoteItem GetItemByID(int id);
        List<QuoteItem> GetItemsByQuoteID(int id);
        List<QuoteItem> GetAllItems(QuoteItemFilterOptions options = null, PagingSettings paging = null);
        bool DeleteItem(QuoteItem quoteItem);
        bool EditItem(QuoteItem quoteItem);

        int Total(QuoteFilterOptions options);
    }
}
