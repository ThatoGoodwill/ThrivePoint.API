using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThrivePoint.Api.Models;

public class User
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required, MaxLength(200)] public string FullName { get; set; } = "";
    [Required, MaxLength(50)] public string Role { get; set; } = "team";
    [MaxLength(10)] public string? AvatarInitials { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Enquiry
{
    public int Id { get; set; }
    [MaxLength(20)] public string? Ref { get; set; }
    [Required, MaxLength(200)] public string Business { get; set; } = "";
    [MaxLength(200)] public string? Contact { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(50)] public string? Phone { get; set; }
    [MaxLength(100)] public string? Industry { get; set; }
    [MaxLength(100)] public string? Location { get; set; }
    public string? Situation { get; set; }
    public string? Challenge { get; set; }
    public string? Goals { get; set; }
    public string? Tried { get; set; }
    public string? Support { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "New";
    [MaxLength(20)] public string Priority { get; set; } = "Medium";
    [MaxLength(50)] public string? Source { get; set; } = "Website";
    [MaxLength(100)] public string? Timeframe { get; set; }
    public decimal Value { get; set; }
    public int? OwnerId { get; set; }
    public User? Owner { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Client
{
    public int Id { get; set; }
    [MaxLength(20)] public string? Ref { get; set; }
    [Required, MaxLength(200)] public string Business { get; set; } = "";
    [MaxLength(200)] public string? Contact { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(50)] public string? Phone { get; set; }
    [MaxLength(100)] public string? Industry { get; set; }
    public List<ClientServiceTag> ServiceTags { get; set; } = new();
    [MaxLength(50)] public string Status { get; set; } = "Onboarding";
    public DateTime Since { get; set; } = DateTime.UtcNow;
    public decimal ContractValue { get; set; }
    public decimal PaidAmount { get; set; }
    public int? OwnerId { get; set; }
    public User? Owner { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ClientServiceTag
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    [Required, MaxLength(100)] public string Name { get; set; } = "";
}

public class Project
{
    public int Id { get; set; }
    [MaxLength(20)] public string? Ref { get; set; }
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public int? ClientId { get; set; }
    public Client? Client { get; set; }
    public DateTime Start { get; set; }
    public DateTime Deadline { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "Not Started";
    public int Progress { get; set; }
    public decimal Value { get; set; }
    public string? Notes { get; set; }
    public List<ProjectTeamMember> Team { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ProjectTeamMember
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class ProjectTask
{
    public int Id { get; set; }
    [MaxLength(20)] public string? Ref { get; set; }
    [Required, MaxLength(200)] public string Title { get; set; } = "";
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public int? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    public DateTime? Due { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "To Do";
    [MaxLength(20)] public string Priority { get; set; } = "Medium";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Proposal
{
    public int Id { get; set; }
    [MaxLength(20)] public string? Ref { get; set; }
    public int? ClientId { get; set; }
    public Client? Client { get; set; }
    public int? EnquiryId { get; set; }
    public Enquiry? Enquiry { get; set; }
    public string Solution { get; set; } = "";
    public List<ProposalServiceTag> ServiceTags { get; set; } = new();
    public decimal Price { get; set; }
    public DateTime? Sent { get; set; }
    public DateTime? Expiry { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "Draft";
    public int? OwnerId { get; set; }
    public User? Owner { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ProposalServiceTag
{
    public int Id { get; set; }
    public int ProposalId { get; set; }
    public Proposal Proposal { get; set; } = null!;
    [Required, MaxLength(100)] public string Name { get; set; } = "";
}

public class ServiceOffering
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    [MaxLength(50)] public string Type { get; set; } = "Project";
    public decimal Price { get; set; }
    [MaxLength(50)] public string Cycle { get; set; } = "One-time";
    public string? Description { get; set; }
}

public class Communication
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string ClientName { get; set; } = "";
    [MaxLength(50)] public string Type { get; set; } = "Call";
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime? FollowUp { get; set; }
    public string? Summary { get; set; }
    public int? OwnerId { get; set; }
    public User? Owner { get; set; }
}

public class ClientDocument
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string ClientName { get; set; } = "";
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    [MaxLength(50)] public string Type { get; set; } = "Other";
    public DateTime Date { get; set; } = DateTime.UtcNow;
    [MaxLength(50)] public string? Size { get; set; }
}

public class CostEntry
{
    public int Id { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    [MaxLength(100)] public string Category { get; set; } = "Other Direct Costs";
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}

public class ActivityLog
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    [MaxLength(50)] public string Icon { get; set; } = "fa-circle-info";
    [MaxLength(20)] public string Color { get; set; } = "#1A5FCC";
    public int? UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class CompanySettings
{
    public int Id { get; set; } = 1;
    [MaxLength(200)] public string Company { get; set; } = "THRIVE-POINT";
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(50)] public string? Phone { get; set; }
    [MaxLength(10)] public string Currency { get; set; } = "R";
    public bool Notifications { get; set; } = true;
    public bool AutoAssign { get; set; }
}