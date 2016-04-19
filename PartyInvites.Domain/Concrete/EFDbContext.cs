using PartyInvites.Domain.Entities;
using System.Data.Entity;

namespace PartyInvites.Domain.Concrete {
    public class EFDbContext : DbContext {

        public EFDbContext() {
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<GuestResponse> GuestResponses { get; set; }
    }
}
