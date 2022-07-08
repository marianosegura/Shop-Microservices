using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastucture.Persistence
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }  // ~Order table object


        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }


        // create/update hook to set create/modified related object fields
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {  // ChangeTracker has the objects being saved (be it created or updated)
            foreach (EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;  // our class object is at .Entity
                        entry.Entity.CreatedBy = "swn";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "swn";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
