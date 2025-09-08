using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Commit.Data.DbExtensions;

public static class DbExtension
{
    private static Dictionary<string, string> constraints = new()
    {
        { "IX_Users_NormalizedEmail", "Email Address" },
        {"IX_Roles_Name", "Role name"},
        {"IX_Permissions_Name", "Permission name"}

    };
    // public static string GetUniqueConstraintViolationMessage(this DbUpdateException ex, IDictionary<string, string>? constraintsToMap = null)
    // {
    //     if (ex.InnerException is not PostgresException { SqlState: "23505" } pgEx) 
    //         return "An error occurred while updating the database.";
    //     var constraintName = pgEx.ConstraintName;
    //     var columnName = string.Empty;
    //     if ((constraintsToMap?.TryGetValue(constraintName ?? string.Empty, out columnName)).GetValueOrDefault())
    //     {
    //         return $"'{columnName}' already exist.";
    //     }
    //     return $"Unique constraint violation on constraint: {constraintName}.";
    //
    // }
    
    public static string GetUniqueConstraintViolationMessage(this DbUpdateException ex)
    {
        // Check if it's a SQL Server exception
        if (ex.InnerException is not SqlException sqlEx || (sqlEx.Number != 2627 && sqlEx.Number != 2601))
            return "An error occurred while updating the database.";

        // Try to extract the constraint or index name from the error message
        var constraintName = ExtractConstraintName(sqlEx.Message);

        if (constraints.TryGetValue(constraintName ?? string.Empty, out var columnName))
        {
            return $"'{columnName}' already exists.";
        }

        return $"Unique constraint violation on constraint: {constraintName ?? "unknown"}";
    }

    private static string? ExtractConstraintName(string message)
    {
        // SQL Server messages usually contain something like:
        // "Violation of UNIQUE KEY constraint 'UQ_Users_Email'. Cannot insert duplicate key in object 'dbo.Users'."
        // We'll try to grab the constraint name between quotes
        var start = message.IndexOf("constraint '", StringComparison.OrdinalIgnoreCase);
        if (start >= 0)
        {
            start += "constraint '".Length;
            var end = message.IndexOf('\'', start);
            if (end > start)
                return message[start..end];
        }
        return null;
    }
}