using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookRight.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreatmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParticipantLimit = table.Column<int>(type: "int", nullable: false),
                    DiscountTypeUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Participants = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssignedTreatments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentRoomLimit = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredTherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Therapists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssociatedClinics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificationTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Therapists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    CertificationRequired = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    ClosingTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningHours_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "Id", "CertificationRequired", "Duration", "MaxParticipants", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("2f1196bb-8d78-444c-bcdb-992806905c6e"), "Physiotherapy", new TimeSpan(0, 0, 30, 0, 0), 1, "Physiotherapy 30", 395m },
                    { new Guid("4401a516-76ba-497f-beba-aee749850ddd"), "Rehabilitation", new TimeSpan(0, 1, 0, 0, 0), 6, "Team Training Rehabilitation", 150m },
                    { new Guid("4ced2638-f883-42fa-ad82-24902b3b1ba4"), "Physiotherapy", new TimeSpan(0, 1, 0, 0, 0), 1, "Physiotherapy 60", 745m },
                    { new Guid("53bb0fec-b7ee-45c7-ae0e-0db7f508658d"), "Physiotherapy", new TimeSpan(0, 0, 45, 0, 0), 1, "Physiotherapy 45", 589m },
                    { new Guid("68a14b26-15d6-4831-a876-221b01260c6b"), "Acupuncture", new TimeSpan(0, 0, 45, 0, 0), 1, "Acupuncture 45", 550m },
                    { new Guid("710cfebd-245c-410d-b63f-9e066f77de39"), "Massage", new TimeSpan(0, 1, 0, 0, 0), 1, "Sports Massage 60", 699m },
                    { new Guid("79b16d43-7475-4409-bd12-ddd0e2067863"), "Dietary", new TimeSpan(0, 1, 0, 0, 0), 1, "Dietary Guidance first time", 799m },
                    { new Guid("b83d128e-f408-40f8-b4ad-25c31250cf2b"), "Massage", new TimeSpan(0, 0, 30, 0, 0), 1, "Sports Massage 30", 350m },
                    { new Guid("f748fc78-c660-4de0-a1a8-f7c0b0930f98"), "Dietary", new TimeSpan(0, 0, 30, 0, 0), 1, "Dietary Guidance follow-up", 450m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_ClinicId",
                table: "OpeningHours",
                column: "ClinicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.DropTable(
                name: "Therapists");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Clinics");
        }
    }
}
