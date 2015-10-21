namespace hAWK.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using hAWK.Data.Common.Models;
    using hAWK.Data.Migrations;
    using hAWK.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class HawkDbContext : IdentityDbContext<User>
    {
        public HawkDbContext()
            : base("HawkDbConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HawkDbContext, Configuration>());
        }

        public static HawkDbContext Create()
        {
            return new HawkDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();

            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in this.ChangeTracker.Entries().Where(e =>
                e.Entity is IAuditInfo &&
                ((e.State == EntityState.Added)
                || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
