using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace SoftOne.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public bool IsCompleted { get; set; }

        [BindProperty]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public IActionResult OnGet(int id)
        {
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
            try
            {
                string connectionString = "Server=PUBUDU-PREMASIR\\SQLEXPRESS;Database=SoftOne;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM TasksManager WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToPage("./Index");
                        }
                        else
                        {
                            return RedirectToPage("./Index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting task: {ex.Message}");
                return Page();
            }
        }
    }
}
