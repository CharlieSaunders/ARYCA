/*using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositorys;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SqliteRepositories.UserRepositories
{
    public static class SqlUserStatsRepository
    {
        public static List<UserStats> GetAll(Guid userReference)
        {
            var response = new List<UserStats>();
            using(var conn = new SqliteConnection("Data source=ARYCA.db"))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText =
                @"
                    SELECT *
                    FROM UserStats
                    WHERE UserReference = $userReference
                ";
                command.Parameters.AddWithValue("$userReference", userReference);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);
                        response.Add(new UserStats(name));
                    }
                }
            }

            return response;
        }
    }
}
*/