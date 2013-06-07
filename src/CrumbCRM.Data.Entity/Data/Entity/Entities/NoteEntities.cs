using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Services;

namespace CrumbCRM.Data.Entity.Entities
{
    public class NoteEntities : EntitiesBase<CrumbCRMEntities>, INoteRepository
    {
        public int Save(Note Note)
        {
            if (Note.ID == 0)
            {
                Context.Note.Add(Note);
            }
            else
            {
                Context.Note.Attach(Note);
                Context.Entry(Note).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return Note.ID;
        }

        public Note GetByID(int id)
        {
            return Context.Note.FirstOrDefault(i => i.ID == id);
        }


        public List<Note> GetByType(int id, NoteType type)
        {
            var Notes = Context.Note.Where(q => !q.Deleted.HasValue);
            Notes = Notes.Where(c => c.Type == type && c.ItemID == id);  

            Notes = Notes.OrderByDescending(x => x.CreatedDate); 
            return Notes.ToList();
        }


        public List<Note> GetAll(Filters.NoteFilterOptions options = null, PagingSettings paging = null)
        {
            var Notes = Context.Note.Where(q => !q.Deleted.HasValue);

            if (options != null)
            {
                if (options.Type.HasValue)
                    Notes = Notes.Where(c => c.Type == options.Type.Value);
            }

            if (options.Order == "desc")
            {
                Notes = Notes.OrderByDescending(x => x.CreatedDate);
            }

            return Notes.ToList();
        }

        public bool Void()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Note Note) {

            if (Note != null)
            {
                Note.Deleted = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Note Note) 
        {
            Context.SaveChanges();
            return true;
        }
    }
}
