using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecShare.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Infrastructure.Data;
public class IdentityApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) :
        base(options)
    { }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
