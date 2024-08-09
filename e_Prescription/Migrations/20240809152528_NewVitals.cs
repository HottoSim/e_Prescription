using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class NewVitals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PatientVitals_Vitals_VitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropTable(
            //    name: "Vitals");

            migrationBuilder.CreateTable(
                name: "Vital",
                columns: table => new
                {
                    VitalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VitalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LowLimit = table.Column<double>(type: "float", nullable: true),
                    HighLimit = table.Column<double>(type: "float", nullable: true),
                    Units = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LowLimitDiastolic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighLimitDiastolic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vital", x => x.VitalId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitals_Vital_VitalId",
                table: "PatientVitals",
                column: "VitalId",
                principalTable: "Vital",
                principalColumn: "VitalId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PatientVitals_Vital_VitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropTable(
            //    name: "Vital");

            migrationBuilder.CreateTable(
                name: "Vitals",
                columns: table => new
                {
                    VitalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HighLimit = table.Column<double>(type: "float", nullable: true),
                    HighLimitDiastolic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowLimit = table.Column<double>(type: "float", nullable: false),
                    LowLimitDiastolic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Units = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VitalName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitals", x => x.VitalId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitals_Vitals_VitalId",
                table: "PatientVitals",
                column: "VitalId",
                principalTable: "Vitals",
                principalColumn: "VitalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
