using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeInvoice.Data;

namespace CreativeInvoice.Services
{
    public class ItemService : ServiceBase<IItemRepository>, IItemService
    {
        public int Save(Item item)
        {
            return Repository.Save(item);
        }

        public List<Item> GetAll(Filters.ItemFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Void()
        {
            return Repository.Void();
        }
    }
}
