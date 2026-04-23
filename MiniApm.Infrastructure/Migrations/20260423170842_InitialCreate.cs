using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Os = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CpuUsage = table.Column<double>(type: "float", nullable: false),
                    CpuCoreCount = table.Column<int>(type: "int", nullable: false),
                    MemoryUsage = table.Column<double>(type: "float", nullable: false),
                    TotalMemory = table.Column<double>(type: "float", nullable: false),
                    AvailableMemory = table.Column<double>(type: "float", nullable: false),
                    UsedMemory = table.Column<double>(type: "float", nullable: false),
                    DiskUsage = table.Column<double>(type: "float", nullable: false),
                    TotalDisk = table.Column<double>(type: "float", nullable: false),
                    FreeDisk = table.Column<double>(type: "float", nullable: false),
                    UsedDisk = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMetrics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemMetrics");
        }
    }
}
