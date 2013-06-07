using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Filters;

namespace CrumbCRM.Services
{
   public interface INoteService
    {
        int Save(Note Note);
        Note GetByID(int id);
        List<Note> GetAll(NoteFilterOptions options = null, PagingSettings paging = null);
        List<Note> GetByType(int id, NoteType type);
        bool Void();
        bool Delete(Note Note);
        bool Edit(Note Note);
    }
}
