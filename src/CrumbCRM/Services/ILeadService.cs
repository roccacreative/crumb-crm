using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Filters;

namespace CrumbCRM.Services
{
   public interface ILeadService
    {
        int Save(Lead Lead);
        Lead GetByID(int id);
        List<Lead> GetAll(LeadFilterOptions options = null, PagingSettings paging = null);
        bool Delete(Lead Lead);
        bool Edit(Lead Lead);

        void AssignTag(Lead lead, Tag tag);

        int Total(LeadFilterOptions options);
    }
}
