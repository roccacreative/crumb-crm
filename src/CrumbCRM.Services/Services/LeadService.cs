using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data;

namespace CrumbCRM.Services
{
    public class LeadService : ServiceBase<ILeadRepository>, ILeadService
    {
        public int Save(Lead Lead)
        {
            return Repository.Save(Lead);
        }

        public Lead GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Lead> GetAll(Filters.LeadFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Delete(Lead Lead) {
            return Repository.Delete(Lead);
        }

        public bool Edit(Lead Lead) 
        {
            return Repository.Edit(Lead);
        }

        public void AssignTag(Lead lead, Tag tag)
        {
            Repository.AssignTag(lead, tag);
        }

        public int Total(Filters.LeadFilterOptions options)
        {
            return Repository.Total(options);
        }
    }
}
