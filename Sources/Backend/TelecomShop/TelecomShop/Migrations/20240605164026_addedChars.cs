using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelecomShop.Migrations
{
    /// <inheritdoc />
    public partial class addedChars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price_one_time_total",
                table: "products");

            migrationBuilder.DropColumn(
                name: "price_recurrent_total",
                table: "products");

            migrationBuilder.DropColumn(
                name: "default_value",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "values",
                table: "characteristics");

            migrationBuilder.AlterColumn<string>(
                name: "upgrade_options",
                table: "products",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "downgrade_options",
                table: "products",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "active_to",
                table: "products",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "active_from",
                table: "products",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "list_values",
                table: "char_involvements",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "voice_amount",
                table: "active_products",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "sms_amount",
                table: "active_products",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "purchace_date",
                table: "active_products",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<float>(
                name: "data_amount",
                table: "active_products",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PriceOneTimeTotal",
                table: "active_products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PriceRecurrentTotal",
                table: "active_products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "data_left",
                table: "active_products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "sms_left",
                table: "active_products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "voice_left",
                table: "active_products",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceOneTimeTotal",
                table: "active_products");

            migrationBuilder.DropColumn(
                name: "PriceRecurrentTotal",
                table: "active_products");

            migrationBuilder.DropColumn(
                name: "data_left",
                table: "active_products");

            migrationBuilder.DropColumn(
                name: "sms_left",
                table: "active_products");

            migrationBuilder.DropColumn(
                name: "voice_left",
                table: "active_products");

            migrationBuilder.AlterColumn<string>(
                name: "upgrade_options",
                table: "products",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "downgrade_options",
                table: "products",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "active_to",
                table: "products",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "active_from",
                table: "products",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "price_one_time_total",
                table: "products",
                type: "real",
                nullable: true,
                defaultValueSql: "0.0");

            migrationBuilder.AddColumn<float>(
                name: "price_recurrent_total",
                table: "products",
                type: "real",
                nullable: true,
                defaultValueSql: "0.0");

            migrationBuilder.AddColumn<string>(
                name: "default_value",
                table: "characteristics",
                type: "character varying",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "values",
                table: "characteristics",
                type: "json",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "list_values",
                table: "char_involvements",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "voice_amount",
                table: "active_products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sms_amount",
                table: "active_products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "purchace_date",
                table: "active_products",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "data_amount",
                table: "active_products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
