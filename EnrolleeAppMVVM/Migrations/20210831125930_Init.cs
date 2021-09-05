using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnrolleeAppMVVM.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EducationName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                });

            migrationBuilder.CreateTable(
                name: "Financing",
                columns: table => new
                {
                    FinancingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancingName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financing", x => x.FinancingId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SpecialityNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialityNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "StudyPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserGUID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastPasswordChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleId = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserGUID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    SpecialityId = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameId = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    SpecialityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyPeriodId = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    MinimumEducationLevel = table.Column<int>(type: "int", nullable: false),
                    FinancingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.SpecialityId);
                    table.ForeignKey(
                        name: "FK_Specialities_Educations_MinimumEducationLevel",
                        column: x => x.MinimumEducationLevel,
                        principalTable: "Educations",
                        principalColumn: "EducationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Specialities_Financing_FinancingId",
                        column: x => x.FinancingId,
                        principalTable: "Financing",
                        principalColumn: "FinancingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Specialities_SpecialityNames_NameId",
                        column: x => x.NameId,
                        principalTable: "SpecialityNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Specialities_StudyPeriods_StudyPeriodId",
                        column: x => x.StudyPeriodId,
                        principalTable: "StudyPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollees",
                columns: table => new
                {
                    EnrolleeGUID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    UserGUID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    PassportId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    EducationId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CertificateNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GPA = table.Column<double>(type: "float", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollees", x => x.EnrolleeGUID);
                    table.ForeignKey(
                        name: "FK_Enrollees_Educations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "EducationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollees_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollees_Users_UserGUID",
                        column: x => x.UserGUID,
                        principalTable: "Users",
                        principalColumn: "UserGUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollees_Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EnrolleeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollees_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollees_Specialities_Enrollees_EnrolleeGUID",
                        column: x => x.EnrolleeGUID,
                        principalTable: "Enrollees",
                        principalColumn: "EnrolleeGUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollees_Specialities_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "SpecialityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "EducationId", "EducationName" },
                values: new object[,]
                {
                    { 2, "Среднее общее образование" },
                    { 1, "Основное общее образование" }
                });

            migrationBuilder.InsertData(
                table: "Financing",
                columns: new[] { "FinancingId", "FinancingName" },
                values: new object[,]
                {
                    { 1, "Бюджетное финансирование" },
                    { 2, "Коммерческое финансирование" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Абитуриент" },
                    { 2, "Администратор" }
                });

            migrationBuilder.InsertData(
                table: "SpecialityNames",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Поварское дело" },
                    { 2, "Программирование" },
                    { 3, "Туризм на базе 9 классов" },
                    { 4, "Туризм на базе 11 классов" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 4, "Ожидается" },
                    { 2, "Подано" },
                    { 1, "Принято" },
                    { 3, "Отказано" }
                });

            migrationBuilder.InsertData(
                table: "StudyPeriods",
                columns: new[] { "Id", "Period" },
                values: new object[,]
                {
                    { 3, "2 года 5 мес." },
                    { 1, "1 год 10 мес." },
                    { 2, "3 года 11 мес." },
                    { 4, "1 год 5 мес." }
                });

            migrationBuilder.InsertData(
                table: "Specialities",
                columns: new[] { "SpecialityId", "FinancingId", "MinimumEducationLevel", "NameId", "SpecialityCode", "StudyPeriodId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, "15.01", 1 },
                    { 2, 2, 1, 1, "15.02", 1 },
                    { 3, 1, 1, 2, "16.01", 2 },
                    { 4, 2, 1, 2, "16.02", 2 },
                    { 5, 1, 1, 3, "17.01", 3 },
                    { 6, 2, 1, 3, "17.02", 3 },
                    { 7, 1, 2, 4, "18.01", 4 },
                    { 8, 2, 2, 4, "18.02", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollees_EducationId",
                table: "Enrollees",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollees_StatusId",
                table: "Enrollees",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollees_UserGUID",
                table: "Enrollees",
                column: "UserGUID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollees_Specialities_EnrolleeGUID",
                table: "Enrollees_Specialities",
                column: "EnrolleeGUID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollees_Specialities_SpecialityId",
                table: "Enrollees_Specialities",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_FinancingId",
                table: "Specialities",
                column: "FinancingId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_MinimumEducationLevel",
                table: "Specialities",
                column: "MinimumEducationLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_NameId",
                table: "Specialities",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_StudyPeriodId",
                table: "Specialities",
                column: "StudyPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollees_Specialities");

            migrationBuilder.DropTable(
                name: "Enrollees");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Financing");

            migrationBuilder.DropTable(
                name: "SpecialityNames");

            migrationBuilder.DropTable(
                name: "StudyPeriods");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
