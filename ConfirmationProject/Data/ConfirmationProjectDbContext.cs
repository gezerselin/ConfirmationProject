using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConfirmationProject.Models;

namespace ConfirmationProject.Data
{
    public class ConfirmationProjectDbContext :DbContext
    {
        public ConfirmationProjectDbContext()
        {

        }
        public ConfirmationProjectDbContext(DbContextOptions<ConfirmationProjectDbContext>options): base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<Gender>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Gender)
                .HasForeignKey(x => x.GenderId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Responses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
                
                

            modelBuilder.Entity<Survey>()
                .HasMany(x => x.Responses)
                .WithOne(x => x.Survey)
                .HasForeignKey(x => x.SurveyId);

                




            base.OnModelCreating(modelBuilder);
        }



    }

}
