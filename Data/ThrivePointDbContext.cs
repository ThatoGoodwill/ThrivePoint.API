using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Data;

public class ThrivePointDbContext : DbContext
{
    public ThrivePointDbContext(DbContextOptions<ThrivePointDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Enquiry> Enquiries => Set<Enquiry>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientServiceTag> ClientServiceTags => Set<ClientServiceTag>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTeamMember> ProjectTeamMembers => Set<ProjectTeamMember>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<ProposalServiceTag> ProposalServiceTags => Set<ProposalServiceTag>();
    public DbSet<ServiceOffering> ServiceOfferings => Set<ServiceOffering>();
    public DbSet<CostEntry> CostEntries => Set<CostEntry>();
    public DbSet<Communication> Communications => Set<Communication>();
    public DbSet<ClientDocument> ClientDocuments => Set<ClientDocument>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<CompanySettings> CompanySettings => Set<CompanySettings>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // decimal precision
        b.Entity<Enquiry>().Property(x => x.Value).HasPrecision(18, 2);
        b.Entity<Client>().Property(x => x.ContractValue).HasPrecision(18, 2);
        b.Entity<Client>().Property(x => x.PaidAmount).HasPrecision(18, 2);
        b.Entity<Project>().Property(x => x.Value).HasPrecision(18, 2);
        b.Entity<Proposal>().Property(x => x.Price).HasPrecision(18, 2);
        b.Entity<ServiceOffering>().Property(x => x.Price).HasPrecision(18, 2);
        b.Entity<CostEntry>().Property(x => x.Amount).HasPrecision(18, 2);

        // unique indexes
        b.Entity<User>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Enquiry>().HasIndex(x => x.Ref).IsUnique();
        b.Entity<Client>().HasIndex(x => x.Ref).IsUnique();

        // friendly table names
        b.Entity<ServiceOffering>().ToTable("ServiceOfferings");
        b.Entity<ClientDocument>().ToTable("ClientDocuments");
        b.Entity<ActivityLog>().ToTable("ActivityLogs");
        b.Entity<CompanySettings>().ToTable("CompanySettings");
    }
}