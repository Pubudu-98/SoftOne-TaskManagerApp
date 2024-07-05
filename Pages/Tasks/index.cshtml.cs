using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace SoftOne.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        public List<TasksInfo> TasksList { get; set; } = new List<TasksInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=PUBUDU-PREMASIR\\SQLEXPRESS;Database=SoftOne;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT Id, Title, Description, IsCompleted, CreatedAt, UpdatedAt FROM TasksManager ORDER BY Id DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TasksInfo task = new TasksInfo
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    IsCompleted = reader.GetBoolean(3),
                                    CreatedAt = reader.GetDateTime(4),
                                    UpdatedAt = reader.GetDateTime(5)
                                };
                                TasksList.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public class TasksInfo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
