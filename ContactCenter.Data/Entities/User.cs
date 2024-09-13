using ContactCenter.Data.Entities;
using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class User
    {
        public User()
        {
            CallCategories = new HashSet<CallCategory>();
            Contacts = new HashSet<Contact>();
            EmailConfigs = new HashSet<EmailConfig>();
            InverseCreator = new HashSet<User>();
            Locations = new HashSet<Location>();
            TicketCategories = new HashSet<TicketCategory>();
            Tickets = new HashSet<Ticket>();
            UserSessions = new HashSet<UserSession>();
            UserExternalLogins = new HashSet<UserExternalLogin>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginId { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string AuthenticatorKey { get; set; }
        public string AuthRecoveryCodes { get; set; }
        public bool TwoFactorAuthEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LockoutExpiryDate { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }

        public virtual User Creator { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual ContactUser ContactUser { get; set; }

        public virtual ICollection<CallCategory> CallCategories { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<EmailConfig> EmailConfigs { get; set; }
        public virtual ICollection<User> InverseCreator { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<TicketCategory> TicketCategories { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<UserSession> UserSessions { get; set; }
        public virtual ICollection<UserExternalLogin> UserExternalLogins { get; set; }

    }
}
