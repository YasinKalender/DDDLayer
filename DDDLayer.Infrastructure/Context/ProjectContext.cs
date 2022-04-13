using DDDLayer.Domain.Entities;
using DDDLayer.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Infrastructure.Context
{
    public class ProjectContext:IdentityDbContext<User,IdentityRole,string>
    {
        public ProjectContext(DbContextOptions<ProjectContext> dbContextOptions):base(dbContextOptions)
        {

        }

        public DbSet<RefreshToken> RefreshToken { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            base.OnModelCreating(modelBuilder);
        }
    }
}
