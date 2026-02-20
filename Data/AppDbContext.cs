using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Diagnostics; // Ye nayi line add karni padi warning ignore karne ke liye
using System.Collections.Generic;

namespace API.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<MemberLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Sirf ye method add kiya hai warning ko ignore karne ke liye
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole {Id = "member-id", Name = "Member", NormalizedName = "MEMBER"},
                    new IdentityRole {Id = "moderator-id", Name = "Moderator", NormalizedName = "MODERATOR"},
                    new IdentityRole {Id = "admin-id", Name = "Admin", NormalizedName = "ADMIN"}
                );

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Recipient)
                    .WithMany(m => m.MessagesReceived)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Sender)
                    .WithMany(m => m.MessagesSent)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberLike>()
                .HasKey(x => new {x.SourceMemberId, x.TargetMemberId});


            modelBuilder.Entity<MemberLike>()
                .HasOne( s => s.TargetMember)
                    .WithMany( t => t.LikedByMembers)
                        .HasForeignKey( s => s.TargetMemberId)
                            .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<MemberLike>()
                .HasOne( s => s.SourceMember)
                    .WithMany( t => t.LikedMembers)
                        .HasForeignKey( s => s.SourceMemberId)
                            .OnDelete(DeleteBehavior.Cascade);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

             var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ?  v.Value.ToUniversalTime() : null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
            );


            foreach(var entityTypes in modelBuilder.Model.GetEntityTypes())
            {
                foreach(var property in entityTypes.GetProperties())
                {
                    if(property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if(property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }
    }
    
}