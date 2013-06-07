using CrumbCRM.Data;
using CrumbCRM.Enums;
using CrumbCRM.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services
{
    public class TagService : ServiceBase<ITagRepository>, ITagService
    {
        public Tag GetByID(int id)
        {
            return Repository.GetByID(id);
        }

        public List<Tag> GetAll(TagFilterOptions options = null)
        {
            return Repository.GetAll(options);
        }

        public int Save(Tag tag)
        {
            return Repository.Save(tag);
        }

        public Tag GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public List<Tag> GetByArea(AreaType? areaType)
        {
            return Repository.GetByArea(areaType);
        }
    }
}
