using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace FAST_DRIVE.Pages.BestDrivers
{
    public class BestDriversModel : PageModel
    {
        public List<DriverInfo> DriversList { get; set; } = new();

        public void OnGet()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT TOP 5 
                        d.driver_id,
                        d.name,
                        COUNT(r.rating_id) AS high_rated_journeys
                    FROM Driver d
                    JOIN Journey j ON d.driver_id = j.driver_id
                    JOIN Rating r ON j.journey_id = r.journey_id
                    WHERE r.rater_type = 'customer_id' AND r.score >= 4.5
                    GROUP BY d.driver_id, d.name
                    ORDER BY high_rated_journeys DESC;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DriversList.Add(new DriverInfo
                        {
                            driver_id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            HighRatedJourneys = reader.GetInt32(2)
                        });
                    }
                }
            }
        }

        public class DriverInfo
        {
            public int driver_id { get; set; }
            public string name { get; set; } = "";
            public int HighRatedJourneys { get; set; }
        }
    }
}
