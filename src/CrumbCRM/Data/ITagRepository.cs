using CrumbCRM.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data
{
    public interface ITagRepository : IDisposable
    {
        Tag GetByID(int id);
        List<Tag> GetAll(TagFilterOptions options = null);
        int Save(Tag tag);

        Tag GetByName(string name);

        List<Tag> GetByArea(Enums.AreaType? areaType);
    }
}
