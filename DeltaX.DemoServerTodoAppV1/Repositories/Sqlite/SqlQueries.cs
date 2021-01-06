namespace DeltaX.DemoServerTodoAppV1.Repositories.Sqlite
{
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Logging;
    using System.Data;

    public class SqlQueries
	{ 
        public static readonly string GetTodoById = "SELECT " +
          "Id, Description, Completed, Created, Updated " +
          "FROM Todos " +
          "WHERE Id = @Id ";

        public static readonly string CountTodo = "SELECT count(*) as Count FROM Todos";

        public static readonly string GetTodosPaged = "SELECT " +
            "Id, Description, Completed, Created, Updated " +
            "FROM Todos " +
            "ORDER By Completed asc, Updated desc " +
            "LIMIT {RowsPerPage} OFFSET {SkipCount} ";
          
        public static readonly string InsertTodo = "INSERT INTO " +
            "Todos (Description, Completed, Created, Updated) " +
            "VALUES (@Description, @Completed, @Created, @Updated); " +
            "SELECT LAST_INSERT_ROWID() AS id; "; 

        public static readonly string DeleteTodoById = "DELETE FROM Todos " +
             "WHERE Id = @Id "; 

        public static readonly string UpdateTodo = "UPDATE Todos SET " +
            "Description = @Description,  " +
            "Completed = @Completed, " +
            "Updated = @Updated " +
            "WHERE Id = @Id "; 

        public static readonly string ScriptCreateTodoTable = @"
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;
 
CREATE TABLE IF NOT EXISTS Todos(
    Id          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Description TEXT    NOT NULL, 
    Completed   BOOLEAN DEFAULT (1) NOT NULL, 
    Created     DATE    DEFAULT (datetime('now', 'localtime') ),
    Updated     DATE    DEFAULT (datetime('now', 'localtime') ) 
); 
COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
"; 
        public static void CreateDatabase(IDbConnection connection, ILogger logger = null)
		{
			logger?.LogInformation("Executing CreateDatabase Script...");

			using (var objCommand = ((SqliteConnection)connection).CreateCommand())
			{
				objCommand.CommandText = ScriptCreateTodoTable;
				var result = objCommand.ExecuteNonQuery();
				logger?.LogInformation("CreateDatabase Execute result {result}", result);
			}
		}
	}
}
