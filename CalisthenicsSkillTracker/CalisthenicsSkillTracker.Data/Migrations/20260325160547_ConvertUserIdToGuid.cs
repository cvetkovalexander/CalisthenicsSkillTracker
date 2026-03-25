using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertUserIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add NewId (Guid) to AspNetUsers
            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            // 2. Add NewUserId (Guid) to custom tables
            migrationBuilder.AddColumn<Guid>(
                name: "NewUserId",
                table: "SkillProgressRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewUserId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: true);

            // 3. Add NewUserId (Guid) to Identity tables
            migrationBuilder.AddColumn<Guid>(
                name: "NewUserId",
                table: "AspNetUserClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewUserId",
                table: "AspNetUserLogins",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewUserId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewUserId",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: true);

            // 4. Copy data: string UserId -> Guid NewId
            migrationBuilder.Sql(@"
                UPDATE sp
                SET sp.NewUserId = u.NewId
                FROM SkillProgressRecords sp
                JOIN AspNetUsers u ON sp.UserId = u.Id;
            ");

            migrationBuilder.Sql(@"
                UPDATE w
                SET w.NewUserId = u.NewId
                FROM Workouts w
                JOIN AspNetUsers u ON w.UserId = u.Id;
            ");

            migrationBuilder.Sql(@"
                UPDATE uc
                SET uc.NewUserId = u.NewId
                FROM AspNetUserClaims uc
                JOIN AspNetUsers u ON uc.UserId = u.Id;
            ");

            migrationBuilder.Sql(@"
                UPDATE ul
                SET ul.NewUserId = u.NewId
                FROM AspNetUserLogins ul
                JOIN AspNetUsers u ON ul.UserId = u.Id;
            ");

            migrationBuilder.Sql(@"
                UPDATE ur
                SET ur.NewUserId = u.NewId
                FROM AspNetUserRoles ur
                JOIN AspNetUsers u ON ur.UserId = u.Id;
            ");

            migrationBuilder.Sql(@"
                UPDATE ut
                SET ut.NewUserId = u.NewId
                FROM AspNetUserTokens ut
                JOIN AspNetUsers u ON ut.UserId = u.Id;
            ");

            // 5. Drop foreign keys referencing AspNetUsers.Id (string)
            migrationBuilder.DropForeignKey(
                name: "FK_SkillProgressRecords_AspNetUsers_UserId",
                table: "SkillProgressRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_AspNetUsers_UserId",
                table: "Workouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            // 6. Drop indexes on UserId where they exist
            migrationBuilder.DropIndex(
                name: "IX_SkillProgressRecords_UserId",
                table: "SkillProgressRecords");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins");
            // AspNetUserRoles & AspNetUserTokens have no UserId index, only PKs

            // 7. Handle composite PK tables first (AspNetUserRoles, AspNetUserTokens)

            // AspNetUserRoles: PK(UserId, RoleId)
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserRoles");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            // AspNetUserTokens: PK(UserId, LoginProvider, Name)
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserTokens");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            // 8. Drop PK on AspNetUsers (string Id)
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            // 9. Replace UserId columns in remaining dependent tables

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SkillProgressRecords");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "SkillProgressRecords",
                newName: "UserId");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Workouts");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "Workouts",
                newName: "UserId");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserClaims");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserLogins");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "AspNetUserLogins",
                newName: "UserId");

            // 10. Replace Id in AspNetUsers

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "AspNetUsers",
                newName: "Id");

            // 11. Recreate PK on AspNetUsers (Guid Id)
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            // 12. Recreate indexes for new Guid UserId columns
            migrationBuilder.CreateIndex(
                name: "IX_SkillProgressRecords_UserId",
                table: "SkillProgressRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            // 13. Recreate foreign keys with Guid UserId
            migrationBuilder.AddForeignKey(
                name: "FK_SkillProgressRecords_AspNetUsers_UserId",
                table: "SkillProgressRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_AspNetUsers_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Drop Guid-based foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_SkillProgressRecords_AspNetUsers_UserId",
                table: "SkillProgressRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_AspNetUsers_UserId",
                table: "Workouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            // 2. Drop indexes on Guid UserId
            migrationBuilder.DropIndex(
                name: "IX_SkillProgressRecords_UserId",
                table: "SkillProgressRecords");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins");

            // 3. Drop PKs on composite tables first

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            // 4. Drop PK on AspNetUsers (Guid Id)
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            // 5. Add back old string Id column
            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // 6. Add back old string UserId columns
            migrationBuilder.AddColumn<string>(
                name: "OldUserId",
                table: "SkillProgressRecords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldUserId",
                table: "Workouts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldUserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldUserId",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldUserId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldUserId",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: true);

            // 7. Copy data back Guid -> string
            migrationBuilder.Sql(@"
                UPDATE AspNetUsers
                SET OldId = CAST(Id AS nvarchar(450));
            ");

            migrationBuilder.Sql(@"
                UPDATE SkillProgressRecords
                SET OldUserId = CAST(UserId AS nvarchar(450));
            ");

            migrationBuilder.Sql(@"
                UPDATE Workouts
                SET OldUserId = CAST(UserId AS nvarchar(450));
            ");

            migrationBuilder.Sql(@"
                UPDATE AspNetUserClaims
                SET OldUserId = CAST(UserId AS nvarchar(450));
            ");

            migrationBuilder.Sql(@"
                UPDATE AspNetUserLogins
                SET OldUserId = CAST(UserId AS nvarchar(450));
            ");

            migrationBuilder.Sql(@"
                UPDATE AspNetUserRoles
                SET OldUserId = CAST(UserId AS nvarchar(450));
            ");

            migrationBuilder.Sql(@"
                UPDATE AspNetUserTokens
                SET OldUserId = CAST(UserId AS nvarchar(450));
            ");

            // 8. Drop Guid UserId columns
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SkillProgressRecords");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUserTokens");

            // 9. Drop Guid Id column in AspNetUsers
            migrationBuilder.DropColumn(
                name: "Id",
                table: "AspNetUsers");

            // 10. Rename OldId -> Id
            migrationBuilder.RenameColumn(
                name: "OldId",
                table: "AspNetUsers",
                newName: "Id");

            // 11. Rename OldUserId -> UserId
            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "SkillProgressRecords",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "Workouts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "AspNetUserTokens",
                newName: "UserId");

            // 12. Make UserId non-nullable again in composite PK tables

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            // 13. Recreate PK on AspNetUsers (string Id)
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            // 14. Recreate PKs on composite tables
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            // 15. Recreate indexes on string UserId
            migrationBuilder.CreateIndex(
                name: "IX_SkillProgressRecords_UserId",
                table: "SkillProgressRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            // 16. Recreate old string-based foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_SkillProgressRecords_AspNetUsers_UserId",
                table: "SkillProgressRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_AspNetUsers_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
