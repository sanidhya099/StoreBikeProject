using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBikeManagmentWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories_CategoryID", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    CouponID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponIDValue = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CouponDiscount = table.Column<int>(type: "int", nullable: false),
                    SalesOrService = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons_CouponID", x => x.CouponID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    City = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Province = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    ContactPhone = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    Textable = table.Column<bool>(type: "bit", nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers_CustomerID", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Major = table.Column<int>(type: "int", nullable: false),
                    Minor = table.Column<int>(type: "int", nullable: false),
                    Build = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Database__3214EC07CD6BBDD7", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions_PositionID", x => x.PositionID);
                });

            migrationBuilder.CreateTable(
                name: "StandardJobs",
                columns: table => new
                {
                    StandardJobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    StandardHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardJobs_StandardJobID", x => x.StandardJobID);
                });

            migrationBuilder.CreateTable(
                name: "UnorderedPurchaseItemCart",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VendorPartNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Unordere__51BCD7976B0A11B8", x => x.CartID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProvinceID = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false, defaultValue: "AB"),
                    PostalCode = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors_VendorID", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerVehicles",
                columns: table => new
                {
                    VehicleIdentification = table.Column<string>(type: "nchar(13)", fixedLength: true, maxLength: 13, nullable: false),
                    Make = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerVehicles_VehicleIdentification", x => x.VehicleIdentification);
                    table.ForeignKey(
                        name: "FK_CustomerVehiclesCustomers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialInsuranceNumber = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: false),
                    LastName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    City = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Province = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    ContactPhone = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Textable = table.Column<bool>(type: "bit", nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    PositionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees_EmployeeID", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_EmployeesPositions_PositionID",
                        column: x => x.PositionID,
                        principalTable: "Positions",
                        principalColumn: "PositionID");
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "smallmoney", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "smallmoney", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false),
                    QuantityOnOrder = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Refundable = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false, defaultValue: "Y"),
                    Discontinued = table.Column<bool>(type: "bit", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts_PartID", x => x.PartID);
                    table.ForeignKey(
                        name: "FK_PartsCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_PartsVendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID");
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDateIn = table.Column<DateTime>(type: "datetime", nullable: false),
                    JobDateStarted = table.Column<DateTime>(type: "datetime", nullable: true),
                    JobDateDone = table.Column<DateTime>(type: "datetime", nullable: true),
                    JobDateOut = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ShopRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    VehicleIdentification = table.Column<string>(type: "nchar(13)", fixedLength: true, maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs_JobID", x => x.JobID);
                    table.ForeignKey(
                        name: "FK_JobsCustomerVehicles_VehicleIdentification",
                        column: x => x.VehicleIdentification,
                        principalTable: "CustomerVehicles",
                        principalColumn: "VehicleIdentification");
                    table.ForeignKey(
                        name: "FK_JobsEmployees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderNumber = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TaxAmount = table.Column<decimal>(type: "money", nullable: false),
                    SubTotal = table.Column<decimal>(type: "money", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders_PurchaseOrderID", x => x.PurchaseOrderID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersEmployees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersVednors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID");
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "money", nullable: false),
                    SubTotal = table.Column<decimal>(type: "money", nullable: false),
                    CouponID = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales_SaleID", x => x.SaleID);
                    table.ForeignKey(
                        name: "FK_SalesCoupons_CouponID",
                        column: x => x.CouponID,
                        principalTable: "Coupons",
                        principalColumn: "CouponID");
                    table.ForeignKey(
                        name: "FK_SalesEmployees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "JobDetails",
                columns: table => new
                {
                    JobDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CouponID = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false, defaultValue: "I"),
                    EmployeeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDetails_JobDetailID", x => x.JobDetailID);
                    table.ForeignKey(
                        name: "FK_JobDetailsCoupons_CouponID",
                        column: x => x.CouponID,
                        principalTable: "Coupons",
                        principalColumn: "CouponID");
                    table.ForeignKey(
                        name: "FK_JobDetailsEmployees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_JobDetailsJobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "JobID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    PurchaseOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "money", nullable: false),
                    VendorPartNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails_PurchaseOrderDetailID", x => x.PurchaseOrderDetailID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetailsParts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetailsPurchaseOrders_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderID");
                });

            migrationBuilder.CreateTable(
                name: "ReceiveOrders",
                columns: table => new
                {
                    ReceiveOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveOrders_ReceiveOrderID", x => x.ReceiveOrderID);
                    table.ForeignKey(
                        name: "FK_ReceiveOrdersPurchaseOrders_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderID");
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    SaleDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails_SaleDetailID", x => x.SaleDetailID);
                    table.ForeignKey(
                        name: "FK_SaleDetailsParts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID");
                    table.ForeignKey(
                        name: "FK_SaleDetailsSalesSaleID",
                        column: x => x.SaleID,
                        principalTable: "Sales",
                        principalColumn: "SaleID");
                });

            migrationBuilder.CreateTable(
                name: "SaleRefunds",
                columns: table => new
                {
                    SaleRefundID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleRefundDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SaleID = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "money", nullable: false),
                    SubTotal = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleRefunds_SaleRefundID", x => x.SaleRefundID);
                    table.ForeignKey(
                        name: "CK_SaleRefundsSales_SaleID",
                        column: x => x.SaleID,
                        principalTable: "Sales",
                        principalColumn: "SaleID");
                    table.ForeignKey(
                        name: "FK_SaleRefundsEmployees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "JobDetailParts",
                columns: table => new
                {
                    JobDetailPartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDetailID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDetailParts_JobPartDetailsID", x => x.JobDetailPartID);
                    table.ForeignKey(
                        name: "FK_JobDetailPartsJobDetails_JobDetailID",
                        column: x => x.JobDetailID,
                        principalTable: "JobDetails",
                        principalColumn: "JobDetailID");
                    table.ForeignKey(
                        name: "FK_JobDetailPartsParts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "ReceiveOrderDetails",
                columns: table => new
                {
                    ReceiveOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiveOrderID = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderDetailID = table.Column<int>(type: "int", nullable: false),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveOrderDetails_ReceiveOrderDetailID", x => x.ReceiveOrderDetailID);
                    table.ForeignKey(
                        name: "FK_ReceiveOrderDetailsPurchaseOrderDetails_OrderDetailID",
                        column: x => x.PurchaseOrderDetailID,
                        principalTable: "PurchaseOrderDetails",
                        principalColumn: "PurchaseOrderDetailID");
                    table.ForeignKey(
                        name: "FK_ReceiveOrderDetailsReceiveOrders_ReceiveOrderID",
                        column: x => x.ReceiveOrderID,
                        principalTable: "ReceiveOrders",
                        principalColumn: "ReceiveOrderID");
                });

            migrationBuilder.CreateTable(
                name: "ReturnedOrderDetails",
                columns: table => new
                {
                    ReturnedOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiveOrderID = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderDetailID = table.Column<int>(type: "int", nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VendorPartNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedOrderDetails_ReturnOrderDetailID", x => x.ReturnedOrderDetailID);
                    table.ForeignKey(
                        name: "FK_ReturnedOrderDetailsPurchaseOrderDetails_OrderDetailID",
                        column: x => x.PurchaseOrderDetailID,
                        principalTable: "PurchaseOrderDetails",
                        principalColumn: "PurchaseOrderDetailID");
                    table.ForeignKey(
                        name: "FK_ReturnedOrderDetailsReceiveOrders_ReceiveOrderID",
                        column: x => x.ReceiveOrderID,
                        principalTable: "ReceiveOrders",
                        principalColumn: "ReceiveOrderID");
                });

            migrationBuilder.CreateTable(
                name: "SaleRefundDetails",
                columns: table => new
                {
                    SaleRefundDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleRefundID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "money", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleRefundDetails_SaleRefundDetailID", x => x.SaleRefundDetailID);
                    table.ForeignKey(
                        name: "FK_SaleRefundDetailsParts_PartId",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID");
                    table.ForeignKey(
                        name: "FK_SaleRefundDetailsSaleRefunds_SaleRefundID",
                        column: x => x.SaleRefundID,
                        principalTable: "SaleRefunds",
                        principalColumn: "SaleRefundID");
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Coupons_CouponIDValue",
                table: "Coupons",
                column: "CouponIDValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVehicles_CustomerID",
                table: "CustomerVehicles",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionID",
                table: "Employees",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_JobDetailParts_JobDetailID",
                table: "JobDetailParts",
                column: "JobDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_JobDetailParts_PartID",
                table: "JobDetailParts",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_JobDetails_CouponID",
                table: "JobDetails",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_JobDetails_EmployeeID",
                table: "JobDetails",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_JobDetails_JobID",
                table: "JobDetails",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmployeeID",
                table: "Jobs",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_VehicleIdentification",
                table: "Jobs",
                column: "VehicleIdentification");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CategoryID",
                table: "Parts",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_VendorID",
                table: "Parts",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PartID",
                table: "PurchaseOrderDetails",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderID",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_EmployeeID",
                table: "PurchaseOrders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorID",
                table: "PurchaseOrders",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveOrderDetails_PurchaseOrderDetailID",
                table: "ReceiveOrderDetails",
                column: "PurchaseOrderDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveOrderDetails_ReceiveOrderID",
                table: "ReceiveOrderDetails",
                column: "ReceiveOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveOrders_PurchaseOrderID",
                table: "ReceiveOrders",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedOrderDetails_PurchaseOrderDetailID",
                table: "ReturnedOrderDetails",
                column: "PurchaseOrderDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedOrderDetails_ReceiveOrderID",
                table: "ReturnedOrderDetails",
                column: "ReceiveOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_PartID",
                table: "SaleDetails",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "UQ_SaleDetails_SaleIDPartID",
                table: "SaleDetails",
                columns: new[] { "SaleID", "PartID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleRefundDetails_PartID",
                table: "SaleRefundDetails",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "UQ_SaleRefundDetails_SaleRefundIDPartID",
                table: "SaleRefundDetails",
                columns: new[] { "SaleRefundID", "PartID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleRefunds_EmployeeID",
                table: "SaleRefunds",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_SaleRefunds_SaleID",
                table: "SaleRefunds",
                column: "SaleID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CouponID",
                table: "Sales",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_EmployeeID",
                table: "Sales",
                column: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseVersion");

            migrationBuilder.DropTable(
                name: "JobDetailParts");

            migrationBuilder.DropTable(
                name: "ReceiveOrderDetails");

            migrationBuilder.DropTable(
                name: "ReturnedOrderDetails");

            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropTable(
                name: "SaleRefundDetails");

            migrationBuilder.DropTable(
                name: "StandardJobs");

            migrationBuilder.DropTable(
                name: "UnorderedPurchaseItemCart");

            migrationBuilder.DropTable(
                name: "JobDetails");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "ReceiveOrders");

            migrationBuilder.DropTable(
                name: "SaleRefunds");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "CustomerVehicles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
