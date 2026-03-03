using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Patient_DateOfBirth",
                table: "Patients");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "Patients",
                type: "BLOB",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldRowVersion: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "Patients",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Patient_DateOfBirth",
                table: "Patients",
                sql: "\"DateOfBirth\" < date('now')");
        }
    }
}
