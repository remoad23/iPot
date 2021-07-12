using Microsoft.EntityFrameworkCore.Migrations;

namespace iPotAPI.Migrations
{
    public partial class InitialCreatePotDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MoistureMinimum = table.Column<float>(type: "REAL", nullable: false),
                    LightMinimum = table.Column<float>(type: "REAL", nullable: false),
                    AutomaticWatering = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutomaticLight = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotificationWater = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotificationLight = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotificationMoisture = table.Column<bool>(type: "INTEGER", nullable: false),
                    UptimeStart = table.Column<string>(type: "TEXT", nullable: true),
                    UptimeEnd = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsId);
                });

            migrationBuilder.CreateTable(
                name: "PlantState",
                columns: table => new
                {
                    PlantStateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SettingsId = table.Column<int>(type: "INTEGER", nullable: false),
                    LedIntensity = table.Column<float>(type: "REAL", nullable: false),
                    AmbientLightIntensity = table.Column<float>(type: "REAL", nullable: false),
                    WaterStorage = table.Column<byte>(type: "INTEGER", nullable: false),
                    MoistureValue = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantState", x => x.PlantStateId);
                    table.ForeignKey(
                        name: "FK_PlantState_Settings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "Settings",
                        principalColumn: "SettingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "SettingsId", "AutomaticLight", "AutomaticWatering", "LightMinimum", "MoistureMinimum", "NotificationLight", "NotificationMoisture", "NotificationWater", "UptimeEnd", "UptimeStart" },
                values: new object[] { 1, true, true, 2f, 2f, true, true, true, "00:20:00", "00:00:00" });

            migrationBuilder.InsertData(
                table: "PlantState",
                columns: new[] { "PlantStateId", "AmbientLightIntensity", "LedIntensity", "MoistureValue", "SettingsId", "WaterStorage" },
                values: new object[] { 1, 30f, 50f, 5f, 1, (byte)50 });

            migrationBuilder.CreateIndex(
                name: "IX_PlantState_SettingsId",
                table: "PlantState",
                column: "SettingsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantState");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
