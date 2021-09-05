using Microsoft.EntityFrameworkCore.Migrations;

namespace EnrolleeAppMVVM.Migrations
{
    public partial class add_new_specialitites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SpecialityNames",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Системное администрирование на базе 11 класса" });

            migrationBuilder.InsertData(
                table: "Specialities",
                columns: new[] { "SpecialityId", "FinancingId", "MinimumEducationLevel", "NameId", "SpecialityCode", "StudyPeriodId" },
                values: new object[] { 9, 2, 2, 5, "19.01", 4 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SpecialityNames",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
