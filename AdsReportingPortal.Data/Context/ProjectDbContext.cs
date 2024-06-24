using AdsReportingPortal.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdsReportingPortal.Data.Context
{
    public class ProjectDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Campaigns> Campaigns { get; set; }
        public DbSet<CampaignStats> CampaignStats { get; set; }
        public DbSet<MetaAgeGenderCategory> MetaAgeGenderCategorys { get; set; }
        public DbSet<VideoPlayAction> VideoPlayActions { get; set; }
        public DbSet<CostPerActionType> CostPerActionTypes { get; set; }
        public DbSet<MetaAction> MetaActions { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }
        public ProjectDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
       
    }

}
