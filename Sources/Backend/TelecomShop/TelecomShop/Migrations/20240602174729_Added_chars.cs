using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TelecomShop.Migrations
{
    /// <inheritdoc />
    public partial class Added_chars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "characteristics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    default_value = table.Column<string>(type: "character varying", nullable: true),
                    values = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("characteristics_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    type = table.Column<string>(type: "character varying", nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    active_from = table.Column<DateOnly>(type: "date", nullable: true),
                    active_to = table.Column<DateOnly>(type: "date", nullable: true),
                    price_one_time = table.Column<float>(type: "real", nullable: true, defaultValueSql: "0.0"),
                    price_recurrent = table.Column<float>(type: "real", nullable: true, defaultValueSql: "0.0"),
                    price_one_time_total = table.Column<float>(type: "real", nullable: true, defaultValueSql: "0.0"),
                    price_recurrent_total = table.Column<float>(type: "real", nullable: true, defaultValueSql: "0.0"),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    upgrade_options = table.Column<string>(type: "json", nullable: true),
                    downgrade_options = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("products_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying", nullable: true),
                    surname = table.Column<string>(type: "character varying", nullable: true),
                    msisdn = table.Column<string>(type: "character varying", nullable: false),
                    password = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "char_involvements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    product_id = table.Column<int>(type: "integer", nullable: true),
                    char_id = table.Column<int>(type: "integer", nullable: true),
                    default_value = table.Column<string>(type: "character varying", nullable: true),
                    list_values = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("char_involvements_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_char_involvements_chars",
                        column: x => x.char_id,
                        principalTable: "characteristics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_char_involvements_ptoducts",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "billing_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<string>(type: "character varying", nullable: true),
                    balance = table.Column<float>(type: "real", nullable: true, defaultValueSql: "0.0"),
                    name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("billing_accounts_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_billing_account_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "active_products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: true),
                    parent_product_id = table.Column<int>(type: "integer", nullable: true),
                    contract_term = table.Column<int>(type: "integer", nullable: true),
                    data_amount = table.Column<int>(type: "integer", nullable: true),
                    voice_amount = table.Column<int>(type: "integer", nullable: true),
                    sms_amount = table.Column<int>(type: "integer", nullable: true),
                    extended_chars = table.Column<string>(type: "json", nullable: true),
                    billing_account_id = table.Column<int>(type: "integer", nullable: true),
                    purchace_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("active_products_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_active_products_billing_acc",
                        column: x => x.billing_account_id,
                        principalTable: "billing_accounts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_active_products_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_active_products_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_active_products_billing_account_id",
                table: "active_products",
                column: "billing_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_active_products_product_id",
                table: "active_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_active_products_user_id",
                table: "active_products",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_billing_accounts_user_id",
                table: "billing_accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_char_involvements_char_id",
                table: "char_involvements",
                column: "char_id");

            migrationBuilder.CreateIndex(
                name: "IX_char_involvements_product_id",
                table: "char_involvements",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "users_unique",
                table: "users",
                column: "msisdn",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "active_products");

            migrationBuilder.DropTable(
                name: "char_involvements");

            migrationBuilder.DropTable(
                name: "billing_accounts");

            migrationBuilder.DropTable(
                name: "characteristics");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
