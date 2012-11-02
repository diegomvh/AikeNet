using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace CoreSQL.Entities
{
    public class ManagedObject<TContext>
        where TContext : ObjectContext, new()
    {

        #region Properties
        protected TContext Context { get; set; }
        #endregion

        #region Object Initialization
        public ManagedObject()
        {
            this.Initialize();
        }

        protected virtual void Initialize()
        {
            // *** Create a default context
            if (this.Context == null)
            {
                this.Context = this.CreateContext();
            }
        }

        protected virtual TContext CreateContext()
        {
            return Activator.CreateInstance<TContext>() as TContext;
        }
        #endregion
    }
}
