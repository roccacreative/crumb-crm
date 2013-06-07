using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data;

namespace CrumbCRM.Services
{
    public class NoteService : ServiceBase<INoteRepository>, INoteService
    {
        public int Save(Note Note)
        {
            return Repository.Save(Note);
        }

        public Note GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Note> GetAll(Filters.NoteFilterOptions options = null, PagingSettings paging = null)
        {
            return Repository.GetAll(options, paging);
        }

        public bool Void()
        {
            return Repository.Void();
        }

        public bool Delete(Note Note) {
            return Repository.Delete(Note);
        }

        public bool Edit(Note Note) 
        {
            return Repository.Edit(Note);
        }


        public List<Note> GetByType(int id, NoteType type)
        {
            return Repository.GetByType(id, type);
        }
    }
}
