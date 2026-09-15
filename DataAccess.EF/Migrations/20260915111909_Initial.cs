using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MatchingCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SearchText = table.Column<string>(type: "longtext", nullable: false),
                    MatchingCriteriaMode = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingCriteria", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionProperties", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "longtext", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestCreateLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RequestState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestCreateLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequestCreateLines_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MatchingCriteriaTransactionCategory",
                columns: table => new
                {
                    CriteriaToMatchOnId = table.Column<int>(type: "int", nullable: false),
                    TransactionCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingCriteriaTransactionCategory", x => new { x.CriteriaToMatchOnId, x.TransactionCategoriesId });
                    table.ForeignKey(
                        name: "FK_MatchingCriteriaTransactionCategory_MatchingCriteria_Criteri~",
                        column: x => x.CriteriaToMatchOnId,
                        principalTable: "MatchingCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchingCriteriaTransactionCategory_TransactionCategories_Tr~",
                        column: x => x.TransactionCategoriesId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MatchingCriteriaTransactionProperty",
                columns: table => new
                {
                    CriteriaToMatchOnId = table.Column<int>(type: "int", nullable: false),
                    TransactionPropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingCriteriaTransactionProperty", x => new { x.CriteriaToMatchOnId, x.TransactionPropertiesId });
                    table.ForeignKey(
                        name: "FK_MatchingCriteriaTransactionProperty_MatchingCriteria_Criteri~",
                        column: x => x.CriteriaToMatchOnId,
                        principalTable: "MatchingCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchingCriteriaTransactionProperty_TransactionProperties_Tr~",
                        column: x => x.TransactionPropertiesId,
                        principalTable: "TransactionProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestDeleteLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RequestState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestDeleteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequestDeleteLines_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestDeleteLines_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestUpdateLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RequestState = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestUpdateLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequestUpdateLines_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestUpdateLines_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionEvidence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FileLocation = table.Column<string>(type: "longtext", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEvidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionEvidence_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionTransactionCategory",
                columns: table => new
                {
                    AssignnedCategoriesId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTransactionCategory", x => new { x.AssignnedCategoriesId, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_TransactionTransactionCategory_TransactionCategories_Assignn~",
                        column: x => x.AssignnedCategoriesId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTransactionCategory_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionTransactionProperty",
                columns: table => new
                {
                    AssignedPropertiesId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTransactionProperty", x => new { x.AssignedPropertiesId, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_TransactionTransactionProperty_TransactionProperties_Assigne~",
                        column: x => x.AssignedPropertiesId,
                        principalTable: "TransactionProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTransactionProperty_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestCreateLineTransactionCategory",
                columns: table => new
                {
                    ChangeRequestCreateLineId = table.Column<int>(type: "int", nullable: false),
                    TransactionCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestCreateLineTransactionCategory", x => new { x.ChangeRequestCreateLineId, x.TransactionCategoriesId });
                    table.ForeignKey(
                        name: "FK_ChangeRequestCreateLineTransactionCategory_ChangeRequestCrea~",
                        column: x => x.ChangeRequestCreateLineId,
                        principalTable: "ChangeRequestCreateLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestCreateLineTransactionCategory_TransactionCatego~",
                        column: x => x.TransactionCategoriesId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestCreateLineTransactionProperty",
                columns: table => new
                {
                    ChangeRequestCreateLineId = table.Column<int>(type: "int", nullable: false),
                    TransactionPropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestCreateLineTransactionProperty", x => new { x.ChangeRequestCreateLineId, x.TransactionPropertiesId });
                    table.ForeignKey(
                        name: "FK_ChangeRequestCreateLineTransactionProperty_ChangeRequestCrea~",
                        column: x => x.ChangeRequestCreateLineId,
                        principalTable: "ChangeRequestCreateLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestCreateLineTransactionProperty_TransactionProper~",
                        column: x => x.TransactionPropertiesId,
                        principalTable: "TransactionProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestUpdateLineTransactionCategory",
                columns: table => new
                {
                    ChangeRequestUpdateLineId = table.Column<int>(type: "int", nullable: false),
                    TransactionCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestUpdateLineTransactionCategory", x => new { x.ChangeRequestUpdateLineId, x.TransactionCategoriesId });
                    table.ForeignKey(
                        name: "FK_ChangeRequestUpdateLineTransactionCategory_ChangeRequestUpda~",
                        column: x => x.ChangeRequestUpdateLineId,
                        principalTable: "ChangeRequestUpdateLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestUpdateLineTransactionCategory_TransactionCatego~",
                        column: x => x.TransactionCategoriesId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeRequestUpdateLineTransactionProperty",
                columns: table => new
                {
                    ChangeRequestUpdateLineId = table.Column<int>(type: "int", nullable: false),
                    TransactionPropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestUpdateLineTransactionProperty", x => new { x.ChangeRequestUpdateLineId, x.TransactionPropertiesId });
                    table.ForeignKey(
                        name: "FK_ChangeRequestUpdateLineTransactionProperty_ChangeRequestUpda~",
                        column: x => x.ChangeRequestUpdateLineId,
                        principalTable: "ChangeRequestUpdateLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestUpdateLineTransactionProperty_TransactionProper~",
                        column: x => x.TransactionPropertiesId,
                        principalTable: "TransactionProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestCreateLines_ChangeRequestId",
                table: "ChangeRequestCreateLines",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestCreateLineTransactionCategory_TransactionCatego~",
                table: "ChangeRequestCreateLineTransactionCategory",
                column: "TransactionCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestCreateLineTransactionProperty_TransactionProper~",
                table: "ChangeRequestCreateLineTransactionProperty",
                column: "TransactionPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDeleteLines_ChangeRequestId",
                table: "ChangeRequestDeleteLines",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDeleteLines_TransactionId",
                table: "ChangeRequestDeleteLines",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestUpdateLines_ChangeRequestId",
                table: "ChangeRequestUpdateLines",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestUpdateLines_TransactionId",
                table: "ChangeRequestUpdateLines",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestUpdateLineTransactionCategory_TransactionCatego~",
                table: "ChangeRequestUpdateLineTransactionCategory",
                column: "TransactionCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestUpdateLineTransactionProperty_TransactionProper~",
                table: "ChangeRequestUpdateLineTransactionProperty",
                column: "TransactionPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingCriteriaTransactionCategory_TransactionCategoriesId",
                table: "MatchingCriteriaTransactionCategory",
                column: "TransactionCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingCriteriaTransactionProperty_TransactionPropertiesId",
                table: "MatchingCriteriaTransactionProperty",
                column: "TransactionPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEvidence_TransactionId",
                table: "TransactionEvidence",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTransactionCategory_TransactionId",
                table: "TransactionTransactionCategory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTransactionProperty_TransactionId",
                table: "TransactionTransactionProperty",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeRequestCreateLineTransactionCategory");

            migrationBuilder.DropTable(
                name: "ChangeRequestCreateLineTransactionProperty");

            migrationBuilder.DropTable(
                name: "ChangeRequestDeleteLines");

            migrationBuilder.DropTable(
                name: "ChangeRequestUpdateLineTransactionCategory");

            migrationBuilder.DropTable(
                name: "ChangeRequestUpdateLineTransactionProperty");

            migrationBuilder.DropTable(
                name: "MatchingCriteriaTransactionCategory");

            migrationBuilder.DropTable(
                name: "MatchingCriteriaTransactionProperty");

            migrationBuilder.DropTable(
                name: "TransactionEvidence");

            migrationBuilder.DropTable(
                name: "TransactionTransactionCategory");

            migrationBuilder.DropTable(
                name: "TransactionTransactionProperty");

            migrationBuilder.DropTable(
                name: "ChangeRequestCreateLines");

            migrationBuilder.DropTable(
                name: "ChangeRequestUpdateLines");

            migrationBuilder.DropTable(
                name: "MatchingCriteria");

            migrationBuilder.DropTable(
                name: "TransactionCategories");

            migrationBuilder.DropTable(
                name: "TransactionProperties");

            migrationBuilder.DropTable(
                name: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
