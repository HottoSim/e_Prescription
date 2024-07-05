using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ContactTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CellphoneNumber = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surbub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_ContactDetails_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientVitals_VitalId",
            //    table: "PatientVitals",
            //    column: "VitalId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Admissions_BedId",
            //    table: "Admissions",
            //    column: "BedId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContactDetails_PatientId",
            //    table: "ContactDetails",
            //    column: "PatientId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Admissions_Beds_BedId",
            //    table: "Admissions",
            //    column: "BedId",
            //    principalTable: "Beds",
            //    principalColumn: "BedId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PatientVitals_Vitals_VitalId",
            //    table: "PatientVitals",
            //    column: "VitalId",
            //    principalTable: "Vitals",
            //    principalColumn: "VitalId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Admissions_Beds_BedId",
            //    table: "Admissions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_PatientVitals_Vitals_VitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropTable(
            //    name: "ContactDetails");

            //migrationBuilder.DropIndex(
            //    name: "IX_PatientVitals_VitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropIndex(
            //    name: "IX_Admissions_BedId",
            //    table: "Admissions");
        }
    }
}
