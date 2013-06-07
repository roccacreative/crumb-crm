using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CrumbCRM.Security;

namespace CrumbCRM.Data.Entity.Model
{
    public class CrumbCRMEntities : DbContext
    {
        public CrumbCRMEntities()
            : base("name=CrumbCRMEntities")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public DbSet<Lead> Lead { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<QuoteItem> QuoteItem { get; set; }
        public DbSet<InvoiceItem> InvoiceItem { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Tag> Tag { get; set; }

        public DbSet<LeadTag> LeadTag { get; set; }
        public DbSet<SaleTag> SaleTag { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasMany(p => p.Tags)
                .WithRequired(t => t.Sale)
                .Map(mc =>
                {
                    mc.ToTable("SaleTags");
                });

            modelBuilder.Entity<Lead>()
                .HasMany(p => p.Tags)
                .WithRequired(t => t.Lead)                
                .Map(mc =>
                {
                    mc.ToTable("LeadTags");
                });

            base.OnModelCreating(modelBuilder);
        }
    }

}
