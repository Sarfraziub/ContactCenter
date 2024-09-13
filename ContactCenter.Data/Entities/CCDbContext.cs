using System;
using System.Collections.Generic;
using ContactCenter.Data.Entities;
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

        //public virtual DbSet<Account> Accounts { get; set; }
        //public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AgentSession> AgentSessions { get; set; }
        public virtual DbSet<Call> Calls { get; set; }
        public virtual DbSet<CallCategory> CallCategories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactUser> ContactUsers { get; set; }

        public virtual DbSet<Councillor> Councillors { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        //public virtual DbSet<EdrsmUser> EdrsmUsers { get; set; }
        public virtual DbSet<EmailConfig> EmailConfigs { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<FaqCategory> FaqCategories { get; set; }
        //public virtual DbSet<IdentificationType> IdentificationTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        //public virtual DbSet<PreferredContactMethod> PreferredContactMethods { get; set; }
        public virtual DbSet<Query> Queries { get; set; }
        public virtual DbSet<RequestedPaymentPlan> RequestedPaymentPlans { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketCategory> TicketCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<UserExternalLogin> UserExternalLogins { get; set; }
        public virtual DbSet<TicketAudit> TicketAudites { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=ContactCenterDB;Username=postgres;Password=admin;Include Error Detail=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Account>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.AccountType).HasColumnName(" AccountType");

            //    entity.Property(e => e.Balance).HasColumnName("Balance ");

            //    entity.Property(e => e.InvoiceNumber)
            //        .IsRequired()
            //        .HasColumnType("character varying")
            //        .HasColumnName("InvoiceNumber ");

            //    entity.Property(e => e.UserId)
            //        .IsRequired()
            //        .HasColumnType("character varying")
            //        .HasColumnName("UserID");
            //});

            //modelBuilder.Entity<Admin>(entity =>
            //{
            //    entity.ToTable("Admin");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.Email)
            //        .IsRequired()
            //        .HasMaxLength(255);

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.Surname)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.Username)
            //        .IsRequired()
            //        .HasMaxLength(128);
            //});
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("Attachment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Container)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

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

                entity.HasIndex(e => e.AgentId, "IX_AgentSession_AgentId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CheckInTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.CheckoutTime).HasColumnType("timestamp without time zone");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.AgentSessions)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AgentSession_AgentId_fkey");
            });
            modelBuilder.Entity<ContactUser>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Citizen_pkey");

                entity.ToTable("ContactUser");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CountryOfOriginId);
                entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false);
                    //.IsRequired()
                    //.HasMaxLength(3);

                entity.Property(e => e.IdentificationNumber)
                    .IsRequired()
                    .HasMaxLength(24);

                entity.Property(e => e.MunicipalityAccountNumber).HasMaxLength(16);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.CountryOfOrigin)
                    .WithMany(p => p.ContactUsers)
                    .HasForeignKey(d => d.CountryOfOriginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Citizen_CountryOfOriginId_fkey");

                entity.HasOne(d => d.ProfilePicture)
                    .WithMany(p => p.ContactUsers)
                    .HasForeignKey(d => d.ProfilePictureId)
                    .HasConstraintName("Citizen_ProfilePictureId_fkey");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.ContactUser)
                    .HasForeignKey<ContactUser>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Citizen_UserId_fkey");
            });
            modelBuilder.Entity<Call>(entity =>
            {
                entity.ToTable("Call");

                entity.HasIndex(e => e.AgentId, "IX_Call_AgentId");

                entity.HasIndex(e => e.CategoryId, "IX_Call_CategoryId");

                entity.HasIndex(e => e.ContactId, "IX_Call_ContactId");

                entity.HasIndex(e => e.LocationId, "IX_Call_LocationId");

                entity.HasIndex(e => e.TicketId, "IX_Call_TicketId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CallerId).HasMaxLength(24);

                entity.Property(e => e.ContactId).HasMaxLength(24);

                entity.Property(e => e.EndTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasDefaultValueSql("''::character varying");

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

                entity.HasIndex(e => e.CreatorId, "IX_CallCategory_CreatorId");

                entity.HasIndex(e => e.ParentId, "IX_CallCategory_ParentId");

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

                entity.HasIndex(e => e.CreatorId, "IX_Contact_CreatorId");

                entity.HasIndex(e => e.LocationId, "IX_Contact_LocationId");

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
            //modelBuilder.Entity<ContactUser>(entity =>
            //{
            //    entity.HasKey(e => e.UserId)
            //        .HasName("Citizen_pkey");

            //    entity.ToTable("ContactUser");

            //    entity.Property(e => e.UserId).ValueGeneratedNever();

            //    entity.Property(e => e.CountryOfOriginId)
            //        .IsRequired()
            //        .HasMaxLength(3);

            //    entity.Property(e => e.IdentificationNumber)
            //        .IsRequired()
            //        .HasMaxLength(24);

            //    entity.Property(e => e.MunicipalityAccountNumber).HasMaxLength(16);

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.Surname)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.HasOne(d => d.CountryOfOrigin)
            //        .WithMany(p => p.ContactUsers)
            //        .HasForeignKey(d => d.CountryOfOriginId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("Citizen_CountryOfOriginId_fkey");

            //    entity.HasOne(d => d.ProfilePicture)
            //        .WithMany(p => p.ContactUsers)
            //        .HasForeignKey(d => d.ProfilePictureId)
            //        .HasConstraintName("Citizen_ProfilePictureId_fkey");

            //    entity.HasOne(d => d.User)
            //        .WithOne(p => p.ContactUser)
            //        .HasForeignKey<ContactUser>(d => d.UserId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("Citizen_UserId_fkey");
            //});

            modelBuilder.Entity<Councillor>(entity =>
            {
                entity.ToTable("Councillor");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasMaxLength(24);

                entity.Property(e => e.Image);

                entity.Property(e => e.CloudinaryPublicId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Ward)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.Id).HasMaxLength(3);

                entity.Property(e => e.DialingCode)
                    .IsRequired()
                    .HasMaxLength(8);
                entity.Property(e => e.CountryCode)
               .IsRequired()
               .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });


            //modelBuilder.Entity<EdrsmUser>(entity =>
            //{
            //    entity.ToTable("EDrsmUser");

            //    entity.Property(e => e.Id).HasColumnType("character varying");

            //    entity.Property(e => e.CellphoneNumber)
            //        .IsRequired()
            //        .HasMaxLength(24);

            //    entity.Property(e => e.Email)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.IdentificationNumber)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.IsAdmin).HasColumnName("IsAdmin ");

            //    entity.Property(e => e.MunicipalityAccountNumber)
            //        .IsRequired()
            //        .HasMaxLength(128)
            //        .HasColumnName("MunicipalityAccountNumber ");

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.PasswordHash)
            //        .IsRequired()
            //        .HasMaxLength(1024);

            //    entity.Property(e => e.Surname)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.Property(e => e.Username)
            //        .IsRequired()
            //        .HasMaxLength(128)
            //        .HasColumnName("Username ");

            //    entity.Property(e => e.Ward)
            //        .IsRequired()
            //        .HasMaxLength(256);
            //});

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

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("Faq");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Answer).IsRequired();

                entity.Property(e => e.Category).IsRequired();

                entity.Property(e => e.ByCategorySorter);

                entity.Property(e => e.Question).IsRequired();
            });

            //modelBuilder.Entity<IdentificationType>(entity =>
            //{
            //    entity.ToTable("IdentificationType");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.IdentificationDocument)
            //        .IsRequired()
            //        .HasColumnType("character varying");
            //});

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.HasIndex(e => e.CreatorId, "IX_Location_CreatorId");

                entity.HasIndex(e => e.ParentId, "IX_Location_ParentId");

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


            modelBuilder.Entity<Query>(entity =>
            {
                entity.HasIndex(e => e.AdminId, "IX_Queries_AdminID");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.QueryText).IsRequired();

                entity.Property(e => e.Status).HasColumnName("Status ");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("UserID");

                //entity.HasOne(d => d.Admin)
                //    .WithMany(p => p.Queries)
                //    .HasForeignKey(d => d.AdminId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("Queries_AdminID_fkey");
            });

            modelBuilder.Entity<RequestedPaymentPlan>(entity =>
            {
                entity.ToTable("RequestedPaymentPlans");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SelectedAccount);
                entity.Property(e => e.ApplicationReference);
                entity.Property(e => e.AmountDue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DepositPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.PaymentPlan);
                entity.Property(e => e.ImpliedMonthlyPayment).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AmountPaidDown).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReasonForPlan);
                entity.Property(e => e.UserId);
                entity.Property(e => e.Name);
                entity.Property(e => e.Surname);
                entity.Property(e => e.MunicipalityAccountNumber);
                entity.Property(e => e.ReviewStatus);
                entity.Property(e => e.ReviewComment);
                entity.Property(e => e.RequestPostedDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.RequestReviewedDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.AdminReviewerId);
                entity.Property(e => e.AdminReviewerName);
                entity.Property(e => e.AdminReviewerSurname);
                entity.Property(e => e.AgreeToTermsAndConditions);
                entity.Property(e => e.CellphoneNumber);
            });

            //modelBuilder.Entity<Ticket>(entity =>
            //{
            //    entity.ToTable("Ticket");

            //    entity.HasIndex(e => e.AssigneeId, "IX_Ticket_AssigneeId");

            //    entity.HasIndex(e => e.CategoryId, "IX_Ticket_CategoryId");

            //    entity.HasIndex(e => e.ContactId, "IX_Ticket_ContactId");

            //    entity.HasIndex(e => e.CreatorId, "IX_Ticket_CreatorId");

            //    entity.HasIndex(e => e.LocationId, "IX_Ticket_LocationId");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.AssignmentDate).HasColumnType("timestamp without time zone");

            //    entity.Property(e => e.ContactId)
            //        .HasMaxLength(24);

            //    entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");

            //    entity.Property(e => e.Number)
            //        .IsRequired()
            //        .HasMaxLength(16)
            //        .HasDefaultValueSql("''::character varying");

            //    entity.Property(e => e.ResolutionDate).HasColumnType("timestamp without time zone");

            //    entity.HasOne(d => d.Assignee)
            //        .WithMany(p => p.Tickets)
            //        .HasForeignKey(d => d.AssigneeId)
            //        .HasConstraintName("Ticket_AssigneeId_fkey");

            //    entity.HasOne(d => d.Category)
            //        .WithMany(p => p.Tickets)
            //        .HasForeignKey(d => d.CategoryId)
            //        .HasConstraintName("Ticket_CategoryId_fkey");

            //    entity.HasOne(d => d.Contact)
            //        .WithMany(p => p.Tickets)
            //        .HasForeignKey(d => d.ContactId)
            //        .OnDelete(DeleteBehavior.SetNull)
            //        .HasConstraintName("Ticket_ContactId_fkey");

            //    entity.HasOne(d => d.Creator)
            //        .WithMany(p => p.Tickets)
            //        .HasForeignKey(d => d.CreatorId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("Ticket_CreatorId_fkey");

            //    entity.HasOne(d => d.Location)
            //        .WithMany(p => p.Tickets)
            //        .HasForeignKey(d => d.LocationId)
            //        .HasConstraintName("Ticket_LocationId_fkey");
            //});

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");
                // Set the primary key
                //entity.HasIndex(e => e.StatusId, "IX_Ticket_StatusId");
                //entity.HasIndex(e => e.AssigneeId, "IX_Ticket_AssigneeId");

                //entity.HasIndex(e => e.CategoryId, "IX_Ticket_CategoryId");

                //entity.HasIndex(e => e.ContactId, "IX_Ticket_ContactId");

                //entity.HasIndex(e => e.CreatorId, "IX_Ticket_CreatorId");

                //entity.HasIndex(e => e.LocationId, "IX_Ticket_LocationId");
                // entity.HasIndex(e => e.IncidentHeadingId, "IX_Incident_IncidentHeadingId");

                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Auto-generate Id values

                // Configure properties
                entity.Property(t => t.ContactId)
                    .HasMaxLength(128);

                entity.Property(t => t.NotesJson)
                    .HasMaxLength(1000);

                entity.Property(t => t.CreationDate)
                    .HasColumnType("timestamp without time zone");

                entity.Property(t => t.AssignmentDate)
                    .HasColumnType("timestamp without time zone");

                entity.Property(t => t.ResolutionDate)
                    .HasColumnType("timestamp without time zone");

                entity.Property(t => t.Description)
                    .HasMaxLength(1000);

                entity.Property(t => t.Number)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(t => t.LocationAddress)
                    .HasMaxLength(500);

                entity.Property(t => t.TicketHeadingId)
                    .IsRequired()
                    .HasMaxLength(50);

                // Configure relationships
                entity.HasOne(t => t.Assignee)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(t => t.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Category)
                     .WithMany(tc => tc.Tickets)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Contact)
                    .WithMany(tc => tc.Tickets)
                    .HasForeignKey(t => t.ContactId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Creator)
                    .WithMany(tc => tc.Tickets)
                    .HasForeignKey(t => t.CreatorId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Location)
                    .WithMany(tc => tc.Tickets)
                    .HasForeignKey(t => t.LocationId)
                    .OnDelete(DeleteBehavior.SetNull);

            });
            modelBuilder.Entity<TicketCategory>(entity =>
            {
                entity.ToTable("TicketCategory");

                entity.HasIndex(e => e.CreatorId, "IX_TicketCategory_CreatorId");

                entity.HasIndex(e => e.ParentId, "IX_TicketCategory_ParentId");

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

                entity.HasIndex(e => e.CreatorId, "IX_User_CreatorId");

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

            modelBuilder.Entity<TicketAudit>(entity =>
            {
                entity.ToTable("TicketAudit");

                entity.HasIndex(e => e.TicketId, "IX_TicketAudit_TicketId");

                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Auto-generate Id values

                entity.Property(e => e.StatusChangeTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.NameOfUpdater).HasMaxLength(128);

                entity.Property(e => e.SurnameOfUpdater).HasMaxLength(128);

                // entity.Property(e => e.ShortSummary).HasMaxLength(256);
             
                entity.Property(e => e.StatusName).HasMaxLength(128);
                entity.Property(e => e.StatusId)
                .HasConversion<int>(); 
                entity.Property(e => e.Description).HasMaxLength(512);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketAudits)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TicketAudit_TicketId_fkey");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
