using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class bhk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ColorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ColorID);
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    CouponID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CouponValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxUsage = table.Column<int>(type: "int", nullable: false),
                    RemainingUsage = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.CouponID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CumstomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CumstomerID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeID);
                });

            migrationBuilder.CreateTable(
                name: "Sole",
                columns: table => new
                {
                    SoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Fabric = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sole", x => x.SoleID);
                });

            migrationBuilder.CreateTable(
                name: "Style",
                columns: table => new
                {
                    StyleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Style", x => x.StyleID);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    VoucherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherCode = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    VoucherValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxUsage = table.Column<int>(type: "int", nullable: false),
                    RemainingUsage = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.VoucherID);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Commune = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CumstomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_Customer_CumstomerID",
                        column: x => x.CumstomerID,
                        principalTable: "Customer",
                        principalColumn: "CumstomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CumstomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CumstomerID);
                    table.ForeignKey(
                        name: "FK_Cart_Customer_CumstomerID",
                        column: x => x.CumstomerID,
                        principalTable: "Customer",
                        principalColumn: "CumstomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoesDetails",
                columns: table => new
                {
                    ShoesDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImportPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ColorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SizeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StyleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoesDetails", x => x.ShoesDetailsId);
                    table.ForeignKey(
                        name: "FK_ShoesDetails_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ColorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoesDetails_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoesDetails_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "SizeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoesDetails_Sole_SoleID",
                        column: x => x.SoleID,
                        principalTable: "Sole",
                        principalColumn: "SoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoesDetails_Style_StyleID",
                        column: x => x.StyleID,
                        principalTable: "Style",
                        principalColumn: "StyleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoesDetails_Supplier_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Supplier",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    SuccessDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    CancelDate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CouponID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.BillID);
                    table.ForeignKey(
                        name: "FK_Bill_Coupon_CouponID",
                        column: x => x.CouponID,
                        principalTable: "Coupon",
                        principalColumn: "CouponID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CumstomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Voucher_VoucherID",
                        column: x => x.VoucherID,
                        principalTable: "Voucher",
                        principalColumn: "VoucherID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartDetails",
                columns: table => new
                {
                    CartDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CumstomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShoesDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetails", x => x.CartDetailsId);
                    table.ForeignKey(
                        name: "FK_CartDetails_Cart_CumstomerID",
                        column: x => x.CumstomerID,
                        principalTable: "Cart",
                        principalColumn: "CumstomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetails_ShoesDetails_ShoesDetailsId",
                        column: x => x.ShoesDetailsId,
                        principalTable: "ShoesDetails",
                        principalColumn: "ShoesDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Image1 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Image2 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Image3 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Image4 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShoesDetailsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_Image_ShoesDetails_ShoesDetailsID",
                        column: x => x.ShoesDetailsID,
                        principalTable: "ShoesDetails",
                        principalColumn: "ShoesDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShoesDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BillDetails_Bill_BillID",
                        column: x => x.BillID,
                        principalTable: "Bill",
                        principalColumn: "BillID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_ShoesDetails_ShoesDetailsId",
                        column: x => x.ShoesDetailsId,
                        principalTable: "ShoesDetails",
                        principalColumn: "ShoesDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CumstomerID",
                table: "Address",
                column: "CumstomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_CouponID",
                table: "Bill",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_CustomerID",
                table: "Bill",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_EmployeeID",
                table: "Bill",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_VoucherID",
                table: "Bill",
                column: "VoucherID");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BillID",
                table: "BillDetails",
                column: "BillID");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_ShoesDetailsId",
                table: "BillDetails",
                column: "ShoesDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CumstomerID",
                table: "CartDetails",
                column: "CumstomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ShoesDetailsId",
                table: "CartDetails",
                column: "ShoesDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_RoleID",
                table: "Employee",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ShoesDetailsID",
                table: "Image",
                column: "ShoesDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesDetails_ColorID",
                table: "ShoesDetails",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesDetails_ProductID",
                table: "ShoesDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesDetails_SizeID",
                table: "ShoesDetails",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesDetails_SoleID",
                table: "ShoesDetails",
                column: "SoleID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesDetails_StyleID",
                table: "ShoesDetails",
                column: "StyleID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesDetails_SupplierID",
                table: "ShoesDetails",
                column: "SupplierID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "CartDetails");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "ShoesDetails");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Sole");

            migrationBuilder.DropTable(
                name: "Style");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
