using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1_Fantasy_liga.Migrations
{
    /// <inheritdoc />
    public partial class addedConstructorImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImagePath",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d95e3850-24bd-4ac9-8e7e-c6d52660cbf8", "AQAAAAIAAYagAAAAEMMQz1GlJqtP0Spr1GxkbeVQhj1o7+EYmTo/M678KYPWOomgwJXwc4oM2rnzpgX8eA==", "eaee702e-3b2e-4c00-bc9c-d6c1aa40a638" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0916556e-42fb-46c1-96dc-4738c4a13714", "AQAAAAIAAYagAAAAENrFvb64OAzvnY5VqEuEJzwNjbBmVpP0AwCWTV739qHOED6E9glOooOBq30GTm+IxQ==", "5fa0d07c-8e56-42ea-a113-679b68599876" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "16e09256-d3df-47d3-9ada-808eec8c5e47", "AQAAAAIAAYagAAAAEKtHA6QIjpH0aSTMuLOKHYarEPrUN0KQHByXR55DjaWZYNk5xODbBgH6XWI1Bxk/IA==", "309587f7-baf5-448a-ba28-54a883c58a4f" });

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImagePath",
                value: null);
        }
    }
}
