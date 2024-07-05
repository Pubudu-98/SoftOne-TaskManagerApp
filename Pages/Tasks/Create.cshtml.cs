using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SoftOne.Pages.Tasks
{
    public class Create : PageModel
    {
        [BindProperty]
        public TaskInputModel Tasks { get; set; } = new TaskInputModel();

        public string? Error { get; set; } // Nullable string for error message

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = "Server=PUBUDU-PREMASIR\\SQLEXPRESS;Database=SoftOne;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to insert into TasksManager table
                    string sql = @"INSERT INTO TasksManager (Title, Description, IsCompleted, CreatedAt, UpdatedAt)
                                   VALUES (@Title, @Description, @IsCompleted, @CreatedAt, @UpdatedAt)";

                    // Create a SqlCommand object
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Title", Tasks.Title);
                        command.Parameters.AddWithValue("@Description", Tasks.Description);
                        command.Parameters.AddWithValue("@IsCompleted", Tasks.IsCompleted);
                        command.Parameters.AddWithValue("@CreatedAt", Tasks.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", Tasks.UpdatedAt);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Redirect to the Index page upon successful insertion
                        return RedirectToPage("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Error = $"An error occurred while creating the task: {ex.Message}";
                return Page();
            }
        }

        public class TaskInputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Title is required.")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "Description is required.")]
            public string Description { get; set; } = string.Empty;

            public bool IsCompleted { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public DateTime UpdatedAt { get; set; } = DateTime.Now;
        }
    }
}
