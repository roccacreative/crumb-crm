using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Enums;
using CrumbCRM.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data.Entity.Entities
{
    public class TagEntities : EntitiesBase<CrumbCRMEntities>, ITagRepository
    {
        public Tag GetByID(int id)
        {
            return Context.Tag.FirstOrDefault(t => t.ID == id);
        }

        public List<Tag> GetAll(TagFilterOptions options = null)
        {
            var tags = Context.Tag.Where(t => !t.Deleted.HasValue);

            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.SearchTerm))
                    tags = tags.Where(t => t.Name.Contains(options.SearchTerm));
            }

            return tags.ToList();
        }

        public int Save(Tag tag)
        {
            if (tag.ID == 0)
            {
                Context.Tag.Add(tag);
            }
            else
            {
                Context.Tag.Attach(tag);
                Context.Entry(tag).State = System.Data.Entity.EntityState.Modified;
            }

            Context.SaveChanges();
            return tag.ID;
        }

        public Tag GetByName(string name)
        {
            return Context.Tag.FirstOrDefault(t => t.Name == name);
        }


        public List<Tag> GetByArea(AreaType? areaType)
        {
            switch (areaType)
            {
                case AreaType.Lead:
                    return Context.Lead.Where(l => !l.Deleted.HasValue).SelectMany(l => l.Tags).Where(t => !t.Tag.Deleted.HasValue).Select(l => l.Tag).ToList();
                case AreaType.Sale:
                    return Context.Sale.Where(l => !l.Deleted.HasValue).SelectMany(l => l.Tags).Where(t => !t.Tag.Deleted.HasValue).Select(l => l.Tag).ToList();
                default:
                    return GetAll();
            }
        }
    }
}
