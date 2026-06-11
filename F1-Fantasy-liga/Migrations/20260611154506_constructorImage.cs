using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1_Fantasy_liga.Migrations
{
    /// <inheritdoc />
    public partial class constructorImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6dc2ad01-f370-442c-a7f2-513308ea0d4b", "AQAAAAIAAYagAAAAEJmD9mIqdiskZc2i71Okmm9reh2kUUhIv/1UbPZTA50pEl1ZnmWfmwxd/+lipIGJ5g==", "911141b0-3758-4027-ad38-3c2847e1e6b9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d72d8312-26c4-4583-877f-f93c3c562920", "AQAAAAIAAYagAAAAEFBqum9H20Q+AfhdV2+BFKXUkBYpQP+uyMiY3AT830+eFbE7jvT/t/GU25RNsmdh4A==", "0e5f2fc7-8562-462f-b58d-5f0e11e6a44e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3b178ee-c662-4939-a80a-e645391bb42f", "AQAAAAIAAYagAAAAEBnLDxqWVFUxeJyKxmObn+icLqf7oe71ND0VlRqrrwlmxIG6JSSEUIcRGmTzbFNRwA==", "4ea075aa-66e0-4bd8-a7f6-06be744118df" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31f14ee4-48de-4afc-bce5-3a0c91f8b596", "AQAAAAIAAYagAAAAEI4c6/U5eyYk3JGdwKTbo/zv5eCrQglDBU882l19YeTiySb+TPRob4s0WFrzTM0Cxw==", "9303f7cc-2ccd-45b6-ac8a-017498652708" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7ac7a69-f6d2-466c-952b-a44c9ffdd548", "AQAAAAIAAYagAAAAEBWiNCo9Sv3W7rwFL1Sta+SsnkRpZkh0pv9YFej3kL+2GHQn8GgQFhnRsSBPfKB5ZA==", "a19cf140-9b4a-49e6-beca-fd9769dbafe4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "756a3965-ff48-474c-a3c2-d35850db2a16", "AQAAAAIAAYagAAAAEN/bcqO6wOuiHMRfiQwKD6JGqE23k5cSs0IDS+ZsbBAB4IVTfr7isa6Q52fIOdvVMg==", "25cc56c4-ef32-4453-9644-62aaf0a94118" });
        }
    }
}
