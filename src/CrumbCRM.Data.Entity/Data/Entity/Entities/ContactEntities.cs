using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Services;
using CrumbCRM.Data.Entity.Extensions;

namespace CrumbCRM.Data.Entity.Entities
{
    public class ContactEntities : EntitiesBase<CrumbCRMEntities>, IContactRepository
    {
        public int Save(Contact contact)
        {
            if (contact.ID == 0)
            {
                Context.Contact.Add(contact);
            }
            else
            {
                Context.Contact.Attach(contact);
                Context.Entry(contact).State = System.Data.Entity.EntityState.Modified;
            }


            Context.SaveChanges();
            return contact.ID;
        }

        public Contact GetByID(int id)
        {
            return Context.Contact.Include("Company").FirstOrDefault(i => i.ID == id);
        }

        public List<Contact> GetAll(Filters.ContactFilterOptions options = null, PagingSettings paging = null)
        {
            IQueryable<Contact> contacts = QueryContacts(options, paging);

            return contacts.ToList();
        }

        private IQueryable<Contact> QueryContacts(Filters.ContactFilterOptions options, PagingSettings paging = null)
        {
            var contacts = Context.Contact.Include("Company").Where(q => !q.Deleted.HasValue);

            if (options != null)
            {
                if (options.Type.HasValue)
                    contacts = contacts.Where(c => c.Type == options.Type);
            }

            if (options != null)
            {
                if (options.Order == "desc")
                    contacts = contacts.OrderByDescending(x => (x.FirstName + x.CompanyName));
                else
                    contacts = contacts.OrderBy(x => (x.FirstName + x.CompanyName));
            }
            else
                contacts = contacts.OrderBy(x => (x.FirstName + x.CompanyName));


            if (paging != null)
            {
                contacts = contacts.Distinct().OrderByDescending(l => l.CreatedDate).ToPagedQueryable(paging);
            }
            return contacts;
        }

        public bool Void()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Contact contact) {

            if (contact != null)
            {
                contact.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Contact contact) 
        {
            Context.SaveChanges();
            return true;
        }

        public int Total(Filters.ContactFilterOptions options)
        {
            return QueryContacts(options).Count();
        }
    }
}
