using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Journey
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public JourneyInfo Journey { get; set; } = new();

        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"INSERT INTO Journey 
                        (journey_id, customer_id, driver_id, vehicle_id,  start_time, end_time, status, cost , created_at, pickup_location, dropoff_location)
                        VALUES 
                        (@journey_id, @customer_id, @driver_id, @vehicle_id,  @start_time, @end_time, @status, @cost , @created_at, @pickup_location, @dropoff_location)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@journey_id", 0); 
                        command.Parameters.AddWithValue("@customer_id", Journey.customer_id);
                        command.Parameters.AddWithValue("@driver_id", Journey.driver_id);
                        command.Parameters.AddWithValue("@vehicle_id", Journey.vehicle_id);
                        command.Parameters.AddWithValue("@start_time", (object?)Journey.start_time ?? DBNull.Value);
                        command.Parameters.AddWithValue("@end_time", (object?)Journey.end_time ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status", Journey.status ?? "");
                        command.Parameters.AddWithValue("@cost", (object?)Journey.cost ?? DBNull.Value);
                        command.Parameters.AddWithValue("@created_at", DateTime.Now);
                        command.Parameters.AddWithValue("@pickup_location", Journey.pickup_location ?? "");
                        command.Parameters.AddWithValue("@dropoff_location", Journey.dropoff_location ?? "");

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Journey created successfully!";
                Journey = new JourneyInfo(); // Reset form
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }

            return Page();
        }

        public class JourneyInfo
        {
            public int journey_id { get; set; }
            public int customer_id { get; set; }
            public int driver_id { get; set; }
            public int vehicle_id { get; set; }

            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
            public string status { get; set; } = "";
            public decimal? cost { get; set; }
            public DateTime created_at { get; set; } = DateTime.Now;
            public string? pickup_location { get; set; }
            public string? dropoff_location { get; set; }
        }
    }
}
