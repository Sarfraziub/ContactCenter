using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContactCenter.Data
{
    public partial class CCDbContext : DbContext
    {
        public CCDbContext()
        {
        }

        public CCDbContext(DbContextOptions<CCDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AgentSession> AgentSessions { get; set; }
        public virtual DbSet<Call> Calls { get; set; }
        public virtual DbSet<CallCategory> CallCategories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<EmailConfig> EmailConfigs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketCategory> TicketCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=CCDb;Username=postgres;Password=pgadmin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.ToTable("Agent");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.HasOne(d => d.Creator)
                    .WithOne(p => p.Agent)
                    .HasForeignKey<Agent>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Agent_Id_fkey");
            });

            modelBuilder.Entity<AgentSession>(entity =>
            {
                entity.ToTable("AgentSession");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CheckInTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.CheckoutTime).HasColumnType("timestamp without time zone");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.AgentSessions)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AgentSession_AgentId_fkey");
            });

            modelBuilder.Entity<Call>(entity =>
            {
                entity.ToTable("Call");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CallerId).HasMaxLength(24);

                entity.Property(e => e.ContactId).HasMaxLength(24);

                entity.Property(e => e.EndTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.StartTime).HasColumnType("timestamp without time zone");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Call_AgentId_fkey");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("Call_CategoryId_fkey");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("Call_ContactId_fkey");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("Call_LocationId_fkey");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("Call_ConversationId_fkey");
            });

            modelBuilder.Entity<CallCategory>(entity =>
            {
                entity.ToTable("CallCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.CallCategories)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CallCategory_CreatorId_fkey");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("CallCategory_ParentId_fkey");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.Id).HasMaxLength(24);

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Contact_CreatorId_fkey");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("Contact_LocationId_fkey");
            });

            modelBuilder.Entity<EmailConfig>(entity =>
            {
                entity.ToTable("EmailConfig");

                entity.HasIndex(e => e.CreatorId, "IX_EmailConfig_CreatorId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Host)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SenderDisplayName).HasMaxLength(128);

                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Username).HasMaxLength(128);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.EmailConfigs)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmailConfig_User");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Location_CreatorId_fkey");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("Location_ParentId_fkey");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AssignmentDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.ContactId)
                    .IsRequired()
                    .HasMaxLength(24);

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.ResolutionDate).HasColumnType("timestamp without time zone");

                entity.HasOne(d => d.Assignee)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.AssigneeId)
                    .HasConstraintName("Ticket_AssigneeId_fkey");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("Ticket_CategoryId_fkey");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Ticket_ContactId_fkey");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Ticket_CreatorId_fkey");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("Ticket_LocationId_fkey");

                entity.HasOne(d => d.TicketStatus)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("Ticket_StatusId_fkey");
            });

            modelBuilder.Entity<TicketCategory>(entity =>
            {
                entity.ToTable("TicketCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.TicketCategories)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TicketCategory_CreatorId_fkey");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("TicketCategory_ParentId_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ActivationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.AuthRecoveryCodes).HasMaxLength(512);

                entity.Property(e => e.AuthenticatorKey).HasMaxLength(128);

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastLoginDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.LockoutExpiryDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Mobile).HasMaxLength(16);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordHash).HasMaxLength(256);

                entity.Property(e => e.SecurityStamp).HasMaxLength(256);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.InverseCreator)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("User_CreatorId_fkey");
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("UserSession");

                entity.HasIndex(e => e.UserId, "IX_UserSession_UserId");

                entity.Property(e => e.ClientIpAddress)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.LoginDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.LogoutDate).HasColumnType("timestamp without time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSessions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("UserSession_UserId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
