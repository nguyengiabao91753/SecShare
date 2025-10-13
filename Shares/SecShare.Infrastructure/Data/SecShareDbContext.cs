using Microsoft.EntityFrameworkCore;
using SecShare.Core.Document;
using SecShare.Core.Group;
using SecShare.Core.Subscription;
using SecShare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Infrastructure.Data;
public class SecShareDbContext : DbContext
{
    public SecShareDbContext(DbContextOptions<SecShareDbContext> options)
            : base(options)
    {
    }

    public DbSet<Documents> Documents { get; set; }
    public DbSet<Share> Shares { get; set; }
    public DbSet<PendingShare> PendingShares { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupShare> GroupShares { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<CurrentStorage> CurrentStorages { get; set; }
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapping table names to match SQL
        modelBuilder.Entity<Documents>().ToTable("Documents");
        modelBuilder.Entity<Share>().ToTable("Shares");
        modelBuilder.Entity<PendingShare>().ToTable("PendingShares");
        modelBuilder.Entity<Group>().ToTable("Groups");
        modelBuilder.Entity<GroupMember>().ToTable("GroupMembers");
        modelBuilder.Entity<GroupShare>().ToTable("GroupShares");
        modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");
        modelBuilder.Entity<CurrentStorage>().ToTable("CurrentStorage");
        modelBuilder.Entity<SubscriptionPlan>().ToTable("SubscriptionPlan");
        modelBuilder.Entity<UserSubscription>().ToTable("UserSubscription");
        modelBuilder.Entity<PaymentTransaction>().ToTable("PaymentTransaction");
    }
}

