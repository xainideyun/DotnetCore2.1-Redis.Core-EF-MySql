using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Redis.Stu.Model.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    GradeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Student_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourse",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    StudentID = table.Column<int>(nullable: true),
                    CourseID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StudentCourse_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourse_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "ID", "CreateTime", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "语文" },
                    { 2, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "数学" },
                    { 3, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "英语" },
                    { 4, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "物理" }
                });

            migrationBuilder.InsertData(
                table: "Grade",
                columns: new[] { "ID", "CreateTime", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "一年级" },
                    { 2, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "二年级" },
                    { 3, new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), "三年级" }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "ID", "Age", "Code", "CreateTime", "GradeId", "Name", "Password", "Remark", "Sex" },
                values: new object[] { 1, 20, "sunquan", new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), 1, "孙权", "000000", null, 1 });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "ID", "Age", "Code", "CreateTime", "GradeId", "Name", "Password", "Remark", "Sex" },
                values: new object[] { 2, 20, "liubei", new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), 1, "刘备", "000000", null, 1 });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "ID", "Age", "Code", "CreateTime", "GradeId", "Name", "Password", "Remark", "Sex" },
                values: new object[] { 3, 20, "caocao", new DateTime(2018, 11, 9, 19, 40, 16, 829, DateTimeKind.Local), 1, "曹操", "000000", null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Student_GradeId",
                table: "Student",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourse_CourseID",
                table: "StudentCourse",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourse_StudentID",
                table: "StudentCourse",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCourse");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Grade");
        }
    }
}
