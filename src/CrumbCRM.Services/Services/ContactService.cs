using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data;

namespace CrumbCRM.Services
{
    public class ContactService : ServiceBase<IContactRepository>, IContactService
    {
        public int Save(Contact contact)
        {
            return Repository.Save(contact);
        }

        public Contact GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Contact> GetAll(Filters.ContactFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Void()
        {
            return Repository.Void();
        }

        public bool Delete(Contact contact) {
            return Repository.Delete(contact);
        }

        public bool Edit(Contact contact) 
        {
            return Repository.Edit(contact);
        }
        public int Total(Filters.ContactFilterOptions options)
        {
            return Repository.Total(options);
        }
    }
}
