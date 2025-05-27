using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Ratings
{
    public class RatingsModel : PageModel
    {
        public List<RatingInfo> RatingsList { get; set; } = new();

        public void OnGet()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"SELECT rating_id, journey_id, rater_id, rater_type, score, comment, timestamp 
                                   FROM Rating ORDER BY rating_id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RatingsList.Add(new RatingInfo
                                {
                                    rating_id = reader.GetInt32(0),
                                    journey_id = reader.GetInt32(1),
                                    rater_id = reader.GetInt32(2),
                                    rater_type = reader.GetString(3),
                                    score = reader.GetDouble(4),
                                    comment = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                    timestamp = reader.IsDBNull(6) ? "" : reader.GetDateTime(6).ToString("dd/MM/yyyy HH:mm")
                                });
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
