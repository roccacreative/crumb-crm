using CrumbCRM.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services
{
    public interface ISaleService
    {
        int Save(Sale Lead);
        Sale GetByID(int id);
        List<Sale> GetAll(SaleFilterOptions options = null, PagingSettings paging = null);
        bool Delete(Sale Lead);
        bool Edit(Sale Lead);

        bool AssignTag(Sale sale, Tag tag);

        int Total(SaleFilterOptions options);
    }
}
