namespace hAWK.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using hAWK.Data.Common.Repository;
    using hAWK.Data.Models;
    using hAWK.Data.Common.Models;

    public class HawkData : IHawkData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public HawkData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public DbContext Context
        {
            get { return this.context; }
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>()
            where T : class
        {
            var typeOfRepository = typeof(T);

            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepo = Activator.CreateInstance(typeof(GenericRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepo);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }

        private IDeletableEntityRepository<T> GetDeleteableRepository<T>()
            where T : class, IDeletableEntity
        {
            var typeOfRepository = typeof(T);

            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepo = Activator.CreateInstance(typeof(DeletableEntityRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepo);
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
