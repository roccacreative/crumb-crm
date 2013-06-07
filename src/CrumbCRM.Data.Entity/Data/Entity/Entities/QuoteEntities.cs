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
    public class QuoteEntities : EntitiesBase<CrumbCRMEntities>, IQuoteRepository
    {
        public int Save(Quote quote)
        {
            if (quote.ID == 0)
            {
                Context.Quote.Add(quote);
            }
            else
            {
                Context.Quote.Attach(quote);
                Context.Entry(quote).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return quote.ID;
        }

        public Quote GetByID(int id)
        {
            return Context.Quote.FirstOrDefault(i => i.ID == id);
        }

        public List<Quote> GetAll(Filters.QuoteFilterOptions options, PagingSettings paging = null)
        {
            var quotes = QueryQuotes(options, paging);
            return quotes.ToList();
        }

        private IQueryable<Quote> QueryQuotes(Filters.QuoteFilterOptions options, PagingSettings paging = null)
        {
            var quotes = Context.Quote.Where(q => !q.Deleted.HasValue);
            if (options != null)
            {

                if (options.Order == "desc")
                {
                    quotes = quotes.OrderByDescending(x => x.CreatedDate);
                }
            }


            if (paging != null)
            {
                quotes = quotes.Distinct().OrderByDescending(l => l.CreatedDate).ToPagedQueryable(paging);
            }

            return quotes;
        }

        public bool Void()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Quote quote) {

            if (quote != null)
            {
                quote.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Quote quote)
        {
            Context.SaveChanges();
            return true;
        }

        public bool EditItem(QuoteItem quoteItem)
        {
            Context.SaveChanges();
            return true;
        }

        public bool DeleteItem(QuoteItem quoteItem)
        {
            if (quoteItem != null)
            {
                quoteItem.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<QuoteItem> GetAllItems(QuoteItemFilterOptions options = null, PagingSettings paging = null)
        {
            return Context.QuoteItem.Where(q => !q.Deleted.HasValue).ToList();
        }

        public QuoteItem GetItemByID(int id)
        {
            return Context.QuoteItem.FirstOrDefault(i => i.ID == id);
        }

        public List<QuoteItem> GetItemsByQuoteID(int id)
        {
            return Context.QuoteItem.Where(i => i.QuoteID == id && !i.Deleted.HasValue).ToList();
        }


        public int SaveItem(QuoteItem quoteItem)
        {
            if (quoteItem.ID == 0)
            {
                Context.QuoteItem.Add(quoteItem);
            }
            else
            {
                Context.QuoteItem.Attach(quoteItem);
                Context.Entry(quoteItem).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return quoteItem.ID;
        }




        public int Total(Filters.QuoteFilterOptions options)
        {
            return QueryQuotes(options).Count();
        }

    }
}
