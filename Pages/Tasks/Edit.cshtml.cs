using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SoftOne.Pages.Tasks
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty, Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [BindProperty, Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public bool IsCompleted { get; set; }

        [BindProperty]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public IActionResult OnGet(int id)
        {
            // Fetch task details from the database based on the id
            try
            {
                string connectionString = "Server=PUBUDU-PREMASIR\\SQLEXPRESS;Database=SoftOne;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM TasksManager WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Id = reader.GetInt32(0);
                                Title = reader.GetString(1);
                                Description = reader.GetString(2);
                                IsCompleted = reader.GetBoolean(3);
                                CreatedAt = reader.GetDateTime(4);
                                UpdatedAt = reader.GetDateTime(5);
                            }
                            else
                            {
                                return RedirectToPage("./Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching task: {ex.Message}");
            }

            return Page();
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

                    // SQL query to update the TasksManager table
                    string sql = @"UPDATE TasksManager
                                   SET Title = @Title,
                                       Description = @Description,
                                       IsCompleted = @IsCompleted,
                                       UpdatedAt = @UpdatedAt
                                   WHERE Id = @Id";

                    // Create a SqlCommand object
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@IsCompleted", IsCompleted);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Redirect to the Index page upon successful update
                        return RedirectToPage("./Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating task: {ex.Message}");
                return Page();
            }
        }
    }
}