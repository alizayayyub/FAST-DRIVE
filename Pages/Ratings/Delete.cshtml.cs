using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Ratings
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public RatingInfo Rating { get; set; } = new();

        public void OnGet(int id)
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT rating_id, journey_id, rater_id, rater_type, score, comment, timestamp FROM Rating WHERE rating_id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Rating.rating_id = reader.GetInt32(0);
                                Rating.journey_id = reader.GetInt32(1);
                                Rating.rater_id = reader.GetInt32(2);
                                Rating.rater_type = reader.GetString(3);
                                Rating.score = reader.GetDouble(4);
                                Rating.comment = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                Rating.timestamp = reader.IsDBNull(6) ? "" : reader.GetDateTime(6).ToString("dd/MM/yyyy HH:mm");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Rating WHERE rating_id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", Rating.rating_id);
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Ratings/Ratings");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }

        public class RatingInfo
        {
            public int rating_id { get; set; }
            public int journey_id { get; set; }
            public int rater_id { get; set; }
            public string rater_type { get; set; } = "";
            public double score { get; set; }
            public string comment { get; set; } = "";
            public string timestamp { get; set; } = "";
        }
    }
}