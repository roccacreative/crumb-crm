using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace CrumbCRM.Services
{
    public abstract class ServiceBase<T> : IDisposable where T : IDisposable
    {
        [Inject]
        public T Repository { get; set; }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}
