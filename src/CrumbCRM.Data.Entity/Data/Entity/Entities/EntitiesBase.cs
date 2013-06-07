using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data.Entity.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    public abstract class EntitiesBase<T1> : IDisposable
        where T1 : DbContext, new()
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public T1 Context { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesBase&lt;T1, T2&gt;"/> class.
        /// </summary>
        //#if DEBUG
        //        public EntitiesBase()
        //            : this(ObjectContextUtils.GetStoreConnection<T2>().CreateObjectContext<T2>())
        //        {
        //
        //        }
        //#else
        public EntitiesBase()
            : this(new T1())
        {

        }
        //#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesBase&lt;T1, T2&gt;"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EntitiesBase(DbContext context)
        {
            this.Context = (T1)context;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
