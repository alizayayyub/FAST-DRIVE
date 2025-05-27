using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace FAST_DRIVE.Pages.Journey
{
    public class JourneyInfo
    {
        public int journey_id { get; set; }
        public int customer_id { get; set; }
        public int driver_id { get; set; }
        public int vehicle_id { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string? status { get; set; }
        public decimal? cost { get; set; }
        public DateTime? created_at { get; set; }
         public string? pickup_location { get; set; }
        public string? dropoff_location { get; set; }
    }

    public class IndexModel : PageModel
    {
        public List<JourneyInfo> JourneyList { get; set; } = new();

        public void OnGet()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Journey";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JourneyList.Add(new JourneyInfo
                            {
                                journey_id = reader.GetInt32(0),
                                customer_id = reader.GetInt32(1),
                                driver_id = reader.GetInt32(2),
                                vehicle_id = reader.GetInt32(3),
                                start_time = reader.GetDateTime(4),
                                end_time = reader.GetDateTime(5),
                                status = reader.GetString(6),
                                cost = reader.GetDecimal(7),
                                created_at = reader.GetDateTime(8),
                                pickup_location = reader.GetString(9),
                                dropoff_location = reader.GetString(10)
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
