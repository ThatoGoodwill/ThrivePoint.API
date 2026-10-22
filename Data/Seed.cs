using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Data;

public static class Seed
{
    public static async Task RunAsync(ThrivePointDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Users.AnyAsync()) return;

        var founder = new User { Email = "founder@thrivepoint.agency",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            FullName = "Bokang Mefane", Role = "founder", AvatarInitials = "BM" };
        var sarah = new User { Email = "sarah@thrivepoint.agency",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("team123"),
            FullName = "Sarah Naidoo", Role = "marketing", AvatarInitials = "SN" };
        var david = new User { Email = "thato@thrivepoint.agency",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("team123"),
            FullName = "Thato Matamane", Role = "developer", AvatarInitials = "TM" };
        db.Users.AddRange(founder, sarah, david);
        await db.SaveChangesAsync();

        var aurora = new Client { Ref = "C-001", Business = "Aurora Dental Studio",
            Contact = "Dr. Lindiwe Khumalo", Industry = "Healthcare",
            Status = "Active", ContractValue = 48000, PaidAmount = 24000,
            OwnerId = founder.Id };
        aurora.ServiceTags.Add(new ClientServiceTag { Name = "Technology Solutions" });
        aurora.ServiceTags.Add(new ClientServiceTag { Name = "Marketing" });

        var bloom = new Client { Ref = "C-002", Business = "Bloom Wellness Co.",
            Contact = "Aisha Patel", Industry = "Wellness",
            Status = "Active", ContractValue = 65000, PaidAmount = 65000,
            OwnerId = david.Id };
        bloom.ServiceTags.Add(new ClientServiceTag { Name = "Marketing Strategy" });

        db.Clients.AddRange(aurora, bloom);
        await db.SaveChangesAsync();

        db.Enquiries.AddRange(
            new Enquiry { Ref = "E-001", Business = "Aurora Dental Studio",
                Contact = "Dr. Lindiwe Khumalo", Email = "hello@auroradental.co.za",
                Industry = "Healthcare", Location = "Sandton", Status = "Qualified",
                Priority = "High", Source = "Website", Value = 48000, OwnerId = founder.Id,
                Support = "Technology Solutions + Marketing" },
            new Enquiry { Ref = "E-002", Business = "Ironclad Logistics",
                Contact = "Johan Steyn", Email = "johan@ironclad.co.za",
                Industry = "Logistics", Location = "Cape Town", Status = "New",
                Priority = "Medium", Source = "Referral", Value = 92000, OwnerId = sarah.Id },
            new Enquiry { Ref = "E-003", Business = "Bloom Wellness Co.",
                Contact = "Aisha Patel", Email = "aisha@bloomwellness.co.za",
                Industry = "Wellness", Location = "Durban", Status = "Proposal",
                Priority = "High", Source = "LinkedIn", Value = 65000, OwnerId = david.Id,
                Support = "Marketing Strategy" }
        );

        db.ServiceOfferings.AddRange(
            new ServiceOffering { Name = "Foundation", Type = "Retainer", Price = 8500, Cycle = "Monthly", Description = "Essential digital presence" },
            new ServiceOffering { Name = "Growth", Type = "Retainer", Price = 18500, Cycle = "Monthly", Description = "Full marketing + tech partnership" },
            new ServiceOffering { Name = "Scale", Type = "Retainer", Price = 35000, Cycle = "Monthly", Description = "Dedicated team, strategy & execution" },
            new ServiceOffering { Name = "Website Build", Type = "Project", Price = 45000, Cycle = "One-time", Description = "Custom website design & development" }
        );

        db.CompanySettings.Add(new CompanySettings { Id = 1 });

        await db.SaveChangesAsync();
    }
}