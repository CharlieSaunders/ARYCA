using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
	public partial class userRole : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Achievements",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Identifier = table.Column<int>(type: "INTEGER", nullable: false),
					Title = table.Column<string>(type: "TEXT", nullable: false),
					Description = table.Column<string>(type: "TEXT", nullable: false),
					ImageUrl = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Achievements", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AppConfig",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					ApplicationName = table.Column<string>(type: "TEXT", nullable: false),
					MemorableDate = table.Column<DateTime>(type: "TEXT", nullable: false),
					ImageText = table.Column<string>(type: "TEXT", nullable: false),
					LoginImageUrl = table.Column<string>(type: "TEXT", nullable: false),
					Balance = table.Column<decimal>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppConfig", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Habits",
				columns: table => new
				{
					Reference = table.Column<Guid>(type: "TEXT", nullable: false),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					Description = table.Column<string>(type: "TEXT", nullable: false),
					Value = table.Column<decimal>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Habits", x => x.Reference);
				});

			migrationBuilder.CreateTable(
				name: "Logs",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Area = table.Column<string>(type: "TEXT", nullable: false),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					Description = table.Column<string>(type: "TEXT", nullable: false),
					Details = table.Column<string>(type: "TEXT", nullable: false),
					Severity = table.Column<string>(type: "TEXT", nullable: false),
					FirstSeen = table.Column<string>(type: "TEXT", nullable: false),
					LastSeen = table.Column<string>(type: "TEXT", nullable: false),
					Occurances = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Logs", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Pledges",
				columns: table => new
				{
					Reference = table.Column<Guid>(type: "TEXT", nullable: false),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					Details = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Pledges", x => x.Reference);
				});

			migrationBuilder.CreateTable(
				name: "Unlockables",
				columns: table => new
				{
					Reference = table.Column<Guid>(type: "TEXT", nullable: false),
					Type = table.Column<string>(type: "TEXT", nullable: false),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					Price = table.Column<decimal>(type: "TEXT", nullable: false),
					Value = table.Column<string>(type: "TEXT", nullable: false),
					Display = table.Column<bool>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Unlockables", x => x.Reference);
				});

			migrationBuilder.CreateTable(
				name: "UserAchievements",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					AchievementReference = table.Column<int>(type: "INTEGER", nullable: false),
					TimesUnlocked = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserAchievements", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserHabits",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					HabitReference = table.Column<Guid>(type: "TEXT", nullable: false),
					LastCompleted = table.Column<DateTime>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserHabits", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserHistoricalStocks",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Time = table.Column<string>(type: "TEXT", nullable: false),
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					Symbol = table.Column<string>(type: "TEXT", nullable: false),
					Shares = table.Column<decimal>(type: "TEXT", nullable: false),
					Profit = table.Column<decimal>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserHistoricalStocks", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserInvestments",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					Symbol = table.Column<string>(type: "TEXT", nullable: false),
					Share = table.Column<decimal>(type: "TEXT", nullable: false),
					Price = table.Column<decimal>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserInvestments", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserPledges",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					AssignerReference = table.Column<Guid>(type: "TEXT", nullable: false),
					AssigneeReference = table.Column<Guid>(type: "TEXT", nullable: false),
					PledgeReference = table.Column<Guid>(type: "TEXT", nullable: false),
					Value = table.Column<decimal>(type: "TEXT", nullable: false),
					AdditionalInformation = table.Column<string>(type: "TEXT", nullable: false),
					AssigneeCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
					AssignerSignedOff = table.Column<bool>(type: "INTEGER", nullable: false),
					AssigneeAccepted = table.Column<bool>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserPledges", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					Username = table.Column<string>(type: "TEXT", nullable: false),
					AuthenticationToken = table.Column<string>(type: "TEXT", nullable: true),
					FirstName = table.Column<string>(type: "TEXT", nullable: false),
					SecondName = table.Column<string>(type: "TEXT", nullable: false),
					AvatarUrl = table.Column<string>(type: "TEXT", nullable: false),
					HasActiveSession = table.Column<bool>(type: "INTEGER", nullable: false),
					Balance = table.Column<decimal>(type: "TEXT", nullable: false),
					Theme = table.Column<string>(type: "TEXT", nullable: false),
					Title = table.Column<string>(type: "TEXT", nullable: false),
					ParticleEffect = table.Column<string>(type: "TEXT", nullable: false),
					FontFamily = table.Column<string>(type: "TEXT", nullable: false),
					Role = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.UserReference);
				});

			migrationBuilder.CreateTable(
				name: "UserStats",
				columns: table => new
				{
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					TradesMade = table.Column<int>(type: "INTEGER", nullable: false),
					HabitsCompleted = table.Column<int>(type: "INTEGER", nullable: false),
					PledgesCompleted = table.Column<int>(type: "INTEGER", nullable: false),
					TradeProfit = table.Column<decimal>(type: "TEXT", nullable: false),
					GiftsGiven = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserStats", x => x.UserReference);
				});

			migrationBuilder.CreateTable(
				name: "UserTransactions",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					Value = table.Column<decimal>(type: "TEXT", nullable: false),
					Type = table.Column<string>(type: "TEXT", nullable: false),
					Added = table.Column<DateTime>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserTransactions", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserUnlockables",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserReference = table.Column<Guid>(type: "TEXT", nullable: false),
					UnlockableReference = table.Column<Guid>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserUnlockables", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Achievements");

			migrationBuilder.DropTable(
				name: "AppConfig");

			migrationBuilder.DropTable(
				name: "Habits");

			migrationBuilder.DropTable(
				name: "Logs");

			migrationBuilder.DropTable(
				name: "Pledges");

			migrationBuilder.DropTable(
				name: "Unlockables");

			migrationBuilder.DropTable(
				name: "UserAchievements");

			migrationBuilder.DropTable(
				name: "UserHabits");

			migrationBuilder.DropTable(
				name: "UserHistoricalStocks");

			migrationBuilder.DropTable(
				name: "UserInvestments");

			migrationBuilder.DropTable(
				name: "UserPledges");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "UserStats");

			migrationBuilder.DropTable(
				name: "UserTransactions");

			migrationBuilder.DropTable(
				name: "UserUnlockables");
		}
	}
}
