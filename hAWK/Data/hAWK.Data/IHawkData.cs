namespace hAWK.Data
{
    using System.Data.Entity;
    using hAWK.Data.Models;
    using hAWK.Data.Common.Repository;

    public interface IHawkData
    {
        DbContext Context { get; }

        IRepository<User> Users { get; }

        int SaveChanges();
    }
}
