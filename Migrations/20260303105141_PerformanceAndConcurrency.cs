using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class PerformanceAndConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Consultations_DoctorId",
                table: "Consultations");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Patients",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_LastName",
                table: "Patients",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_DoctorId_Date",
                table: "Consultations",
                columns: new[] { "DoctorId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patient_LastName",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Consultation_DoctorId_Date",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_DoctorId",
                table: "Consultations",
                column: "DoctorId");
        }
    }
}
