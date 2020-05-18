using Microsoft.EntityFrameworkCore;

namespace GM.Common
{
    public partial class BizDataContext : DbContext
    {
        public BizDataContext(DbContextOptions<BizDataContext> options) : base(options)
        {

        }

        public BizDataContext()
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Setting.DbConnection);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ViewContent>().HasKey(t => new { t.ModelID, t.Language });
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
