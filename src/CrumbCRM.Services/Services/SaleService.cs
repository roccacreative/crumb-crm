using CrumbCRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services
{
    public class SaleService : ServiceBase<ISaleRepository>, ISaleService
    {
        public int Save(Sale sale)
        {
            return Repository.Save(sale);
        }

        public Sale GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Sale> GetAll(Filters.SaleFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Delete(Sale sale)
        {
            return Repository.Delete(sale);
        }

        public bool Edit(Sale sale)
        {
            return Repository.Edit(sale);
        }

        public bool AssignTag(Sale sale, Tag tag)
        {
            return Repository.AssignTag(sale, tag);
        }

        public int Total(Filters.SaleFilterOptions options)
        {
            return Repository.Total(options);
        }


        public decimal Sum(Filters.SaleFilterOptions options)
        {
            return Repository.Sum(options);
        }
    }
}
