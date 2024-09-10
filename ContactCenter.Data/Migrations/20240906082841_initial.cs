using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContactCenter.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    UniqueId = table.Column<Guid>(type: "uuid", nullable: true),
                    Container = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Councillor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ContactNumber = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Ward = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    CloudinaryPublicId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Councillor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", maxLength: 3, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DialingCode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqCategories_FaqCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FaqCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Queries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    QueryText = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(name: "Status ", type: "integer", nullable: false),
                    AdminID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestedPaymentPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedAccount = table.Column<string>(type: "text", nullable: true),
                    ApplicationReference = table.Column<string>(type: "text", nullable: true),
                    AmountDue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DepositPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PaymentPlan = table.Column<string>(type: "text", nullable: true),
                    ImpliedMonthlyPayment = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AmountPaidDown = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReasonForPlan = table.Column<string>(type: "text", nullable: true),
                    RequestPostedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    MunicipalityAccountNumber = table.Column<string>(type: "text", nullable: true),
                    CellphoneNumber = table.Column<string>(type: "text", nullable: true),
                    AgreeToTermsAndConditions = table.Column<bool>(type: "boolean", nullable: false),
                    ReviewStatus = table.Column<string>(type: "text", nullable: true),
                    ReviewComment = table.Column<string>(type: "text", nullable: true),
                    RequestReviewedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AdminReviewerId = table.Column<string>(type: "text", nullable: true),
                    AdminReviewerName = table.Column<string>(type: "text", nullable: true),
                    AdminReviewerSurname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedPaymentPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LoginId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Mobile = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AuthenticatorKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    AuthRecoveryCodes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TwoFactorAuthEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    LockoutExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "User_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    ByCategorySorter = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    FaqCategoryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faq_FaqCategories_FaqCategoryId",
                        column: x => x.FaqCategoryId,
                        principalTable: "FaqCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketHeading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false),
                    HeadingName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHeading", x => x.Id);
                    table.ForeignKey(
                        name: "TicketHeading_TicketTypeId_fkey",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Extension = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.Id);
                    table.ForeignKey(
                        name: "Agent_Id_fkey",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CallCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallCategory", x => x.Id);
                    table.ForeignKey(
                        name: "CallCategory_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "CallCategory_ParentId_fkey",
                        column: x => x.ParentId,
                        principalTable: "CallCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Surname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    MunicipalityAccountNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    PreferredContactMethodId = table.Column<int>(type: "integer", nullable: false),
                    IdentificationTypeId = table.Column<int>(type: "integer", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    AgreedToTerms = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfilePictureId = table.Column<Guid>(type: "uuid", nullable: true),
                    CountryOfOriginId = table.Column<int>(type: "integer", nullable: false),
                    Ward = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Citizen_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Citizen_CountryOfOriginId_fkey",
                        column: x => x.CountryOfOriginId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Citizen_ProfilePictureId_fkey",
                        column: x => x.ProfilePictureId,
                        principalTable: "Attachment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Citizen_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TargetId = table.Column<int>(type: "integer", nullable: false),
                    SenderId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SenderDisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Host = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    EnableSsl = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailConfig_User",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    GeoLatitude = table.Column<decimal>(type: "numeric", nullable: true),
                    GeoLongitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "Location_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Location_ParentId_fkey",
                        column: x => x.ParentId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCategory", x => x.Id);
                    table.ForeignKey(
                        name: "TicketCategory_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "TicketCategory_ParentId_fkey",
                        column: x => x.ParentId,
                        principalTable: "TicketCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserExternalLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginId = table.Column<string>(type: "text", nullable: true),
                    ProviderKey = table.Column<string>(type: "text", nullable: true),
                    AuthSchemeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PictureUrl = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExternalLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExternalLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientIpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LoginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogoutDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AuthSchemeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSession", x => x.Id);
                    table.ForeignKey(
                        name: "UserSession_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CheckoutTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentSession", x => x.Id);
                    table.ForeignKey(
                        name: "AgentSession_AgentId_fkey",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DetailsJson = table.Column<string>(type: "text", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ContactUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactUserId1 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "Contact_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Contact_LocationId_fkey",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contact_ContactUser_ContactUserId1",
                        column: x => x.ContactUserId1,
                        principalTable: "ContactUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    NotesJson = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignmentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ResolutionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''::character varying"),
                    LocationAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TicketHeadingId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_TicketHeading_TicketHeadingId",
                        column: x => x.TicketHeadingId,
                        principalTable: "TicketHeading",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Ticket_AssigneeId_fkey",
                        column: x => x.AssigneeId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Ticket_CategoryId_fkey",
                        column: x => x.CategoryId,
                        principalTable: "TicketCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Ticket_ContactId_fkey",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Ticket_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Ticket_LocationId_fkey",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Call",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DirectionId = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    NotesJson = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false, defaultValueSql: "''::character varying"),
                    CallerId = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Call", x => x.Id);
                    table.ForeignKey(
                        name: "Call_AgentId_fkey",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Call_CategoryId_fkey",
                        column: x => x.CategoryId,
                        principalTable: "CallCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Call_ContactId_fkey",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Call_ConversationId_fkey",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Call_LocationId_fkey",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    StatusName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    StatusChangeTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    NameOfUpdater = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SurnameOfUpdater = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ShortSummary = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DetailedDescription = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAudit_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TicketAudit_StatusId_fkey",
                        column: x => x.StatusId,
                        principalTable: "TicketStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentSession_AgentId",
                table: "AgentSession",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Call_AgentId",
                table: "Call",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Call_CategoryId",
                table: "Call",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Call_ContactId",
                table: "Call",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Call_LocationId",
                table: "Call",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Call_TicketId",
                table: "Call",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_CallCategory_CreatorId",
                table: "CallCategory",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CallCategory_ParentId",
                table: "CallCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_ContactUserId1",
                table: "Contact",
                column: "ContactUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CreatorId",
                table: "Contact",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_LocationId",
                table: "Contact",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUser_CountryOfOriginId",
                table: "ContactUser",
                column: "CountryOfOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUser_ProfilePictureId",
                table: "ContactUser",
                column: "ProfilePictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUser_UserId",
                table: "ContactUser",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfig_CreatorId",
                table: "EmailConfig",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Faq_FaqCategoryId",
                table: "Faq",
                column: "FaqCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqCategories_ParentId",
                table: "FaqCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CreatorId",
                table: "Location",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ParentId",
                table: "Location",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Queries_AdminID",
                table: "Queries",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AssigneeId",
                table: "Ticket",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CategoryId",
                table: "Ticket",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ContactId",
                table: "Ticket",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CreatorId",
                table: "Ticket",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_LocationId",
                table: "Ticket",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_StatusId",
                table: "Ticket",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketHeadingId",
                table: "Ticket",
                column: "TicketHeadingId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAudit_StatusId",
                table: "TicketAudit",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAudit_TicketId",
                table: "TicketAudit",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategory_CreatorId",
                table: "TicketCategory",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategory_ParentId",
                table: "TicketCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHeading_TicketTypeId",
                table: "TicketHeading",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatorId",
                table: "User",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalLogins_UserId",
                table: "UserExternalLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_UserId",
                table: "UserSession",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentSession");

            migrationBuilder.DropTable(
                name: "Call");

            migrationBuilder.DropTable(
                name: "Councillor");

            migrationBuilder.DropTable(
                name: "EmailConfig");

            migrationBuilder.DropTable(
                name: "Faq");

            migrationBuilder.DropTable(
                name: "Queries");

            migrationBuilder.DropTable(
                name: "RequestedPaymentPlans");

            migrationBuilder.DropTable(
                name: "TicketAudit");

            migrationBuilder.DropTable(
                name: "UserExternalLogins");

            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.DropTable(
                name: "CallCategory");

            migrationBuilder.DropTable(
                name: "FaqCategories");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TicketStatus");

            migrationBuilder.DropTable(
                name: "TicketHeading");

            migrationBuilder.DropTable(
                name: "TicketCategory");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "ContactUser");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
