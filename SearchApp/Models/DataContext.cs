using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SearchApp.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Domains> Domain { get; set; }
        public DbSet<Types> Type { get; set; }
        public DbSet<Attributes> Attribute { get; set; }
        

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            try
            {
                objectContext.Detach(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            /* Remove english plural */
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Rename generated relational Tables
            modelBuilder.Entity<Domains>()
                .HasMany(m => m.Types).WithMany(s => s.Domains)
                .Map(t => t.MapLeftKey("DomainID")
                    .MapRightKey("TypeID")
                    .ToTable("DomainToType"));

            modelBuilder.Entity<Types>()
                .HasMany(m => m.Attributes).WithMany(s => s.Types)
                .Map(t => t.MapLeftKey("TypeID")
                    .MapRightKey("AttributeID")
                    .ToTable("TypeToAttribute"));
        }
    }
}