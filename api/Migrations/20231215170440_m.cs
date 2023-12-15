using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    province_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    province_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UnitPrices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    weightLimit = table.Column<double>(type: "float", nullable: false),
                    chargeRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitPrices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    district_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    province_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_province_id",
                        column: x => x.province_id,
                        principalTable: "Provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    shippingAddId = table.Column<int>(type: "int", nullable: false),
                    deilveryAddId = table.Column<int>(type: "int", nullable: false),
                    unitPriceId = table.Column<int>(type: "int", nullable: false),
                    charge = table.Column<double>(type: "float", nullable: false),
                    pickupType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deliverType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    insuranceFee = table.Column<double>(type: "float", nullable: false),
                    totalCharge = table.Column<double>(type: "float", nullable: false),
                    payer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cod = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bills_UnitPrices_unitPriceId",
                        column: x => x.unitPriceId,
                        principalTable: "UnitPrices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    ward_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    district_id = table.Column<int>(type: "int", nullable: false),
                    location_code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wards_Districts_district_id",
                        column: x => x.district_id,
                        principalTable: "Districts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    billId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false),
                    length = table.Column<int>(type: "int", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_BillDetails_Bills_billId",
                        column: x => x.billId,
                        principalTable: "Bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsigneeAddresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsigneeAddresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_ConsigneeAddresses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsigneeAddresses_Wards_wardId",
                        column: x => x.wardId,
                        principalTable: "Wards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostOffices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    wardId = table.Column<int>(type: "int", nullable: false),
                    postCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    longtitude = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostOffices", x => x.id);
                    table.ForeignKey(
                        name: "FK_PostOffices_Wards_wardId",
                        column: x => x.wardId,
                        principalTable: "Wards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipperAddresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperAddresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_ShipperAddresses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipperAddresses_Wards_wardId",
                        column: x => x.wardId,
                        principalTable: "Wards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullname = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    postOfficeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_Employees_PostOffices_postOfficeId",
                        column: x => x.postOfficeId,
                        principalTable: "PostOffices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    billId = table.Column<int>(type: "int", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.id);
                    table.ForeignKey(
                        name: "FK_Status_Bills_billId",
                        column: x => x.billId,
                        principalTable: "Bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Status_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_billId",
                table: "BillDetails",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_unitPriceId",
                table: "Bills",
                column: "unitPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_userId",
                table: "Bills",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsigneeAddresses_userId",
                table: "ConsigneeAddresses",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsigneeAddresses_wardId",
                table: "ConsigneeAddresses",
                column: "wardId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_province_id",
                table: "Districts",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_postOfficeId",
                table: "Employees",
                column: "postOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_roleId",
                table: "Employees",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_roleId",
                table: "Permissions",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_wardId",
                table: "PostOffices",
                column: "wardId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipperAddresses_userId",
                table: "ShipperAddresses",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipperAddresses_wardId",
                table: "ShipperAddresses",
                column: "wardId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_billId",
                table: "Status",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_employeeId",
                table: "Status",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_district_id",
                table: "Wards",
                column: "district_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "ConsigneeAddresses");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "ShipperAddresses");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "UnitPrices");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PostOffices");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
