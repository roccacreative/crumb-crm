using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data;

namespace CrumbCRM.Services
{
    public class QuoteService : ServiceBase<IQuoteRepository>, IQuoteService
    {
        public int Save(Quote quote)
        {
            return Repository.Save(quote);
        }

        public Quote GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Quote> GetAll(Filters.QuoteFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Void()
        {
            return Repository.Void();
        }

        public bool Delete(Quote quote)
        {
            return Repository.Delete(quote);
        }
        public bool Edit(Quote quote)
        {
            return Repository.Edit(quote);
        }


        // items

        public int SaveItem(QuoteItem quoteItem)
        {
            return Repository.SaveItem(quoteItem);
        }

        public bool DeleteItem(QuoteItem quoteItem)
        {
            return Repository.DeleteItem(quoteItem);
        }

        public QuoteItem GetItemByID(int id)
        {
            return Repository.GetItemByID(id);
        }
        public List<QuoteItem> GetItemsByQuoteID(int id)
        {
            return Repository.GetItemsByQuoteID(id);
        }

        public List<QuoteItem> GetAllItems(Filters.QuoteItemFilterOptions options = null, PagingSettings paging = null)
        {
             return Repository.GetAllItems(options, paging);
        }

        public bool EditItem(QuoteItem quoteItem)
        {
            return Repository.EditItem(quoteItem);
        }
        public int Total(Filters.QuoteFilterOptions options)
        {
            return Repository.Total(options);
        }

    }
}
