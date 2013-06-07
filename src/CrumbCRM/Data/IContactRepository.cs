using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Filters;

namespace CrumbCRM.Data
{
    public interface IContactRepository : IRepositoryBase
    {
        int Save(Contact contact);
        Contact GetByID(int id);
        List<Contact> GetAll(ContactFilterOptions options = null, PagingSettings paging = null);
        bool Void();
        bool Delete(Contact contact);
        bool Edit(Contact contact);

        int Total(ContactFilterOptions options);
    }
}
