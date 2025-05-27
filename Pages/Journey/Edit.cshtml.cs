using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Journey
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public JourneyInfo Journey { get; set; } = new();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Journey WHERE journey_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Journey.journey_id = reader.GetInt32(0);
                                Journey.customer_id = reader.GetInt32(1);
                                Journey.driver_id = reader.GetInt32(2);
                                Journey.vehicle_id = reader.GetInt32(3);
                                Journey.start_time = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                                Journey.end_time = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                                Journey.status = reader.GetString(6);
                                Journey.cost = reader.IsDBNull(7) ? null : reader.GetDecimal(7);
                                Journey.created_at = reader.GetDateTime(8);
                                Journey.pickup_location = reader.GetString(9);
                                Journey.dropoff_location = reader.GetString(10);
                            }
                            else
                            {
                                ErrorMessage = "Journey not found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
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
                    string sql = @"UPDATE Journey SET 
                        customer_id = @customer_id,
                        driver_id = @driver_id,
                        vehicle_id = @vehicle_id,
                        start_time = @start_time,
                        end_time = @end_time,
                        status = @status,
                        cost = @cost,
                        pickup_location = @pickup_location,
                        dropoff_location = @dropoff_location
                        WHERE journey_id = @journey_id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@journey_id", Journey.journey_id);
                        command.Parameters.AddWithValue("@customer_id", Journey.customer_id);
                        command.Parameters.AddWithValue("@driver_id", Journey.driver_id);
                        command.Parameters.AddWithValue("@vehicle_id", Journey.vehicle_id);
                        command.Parameters.AddWithValue("@start_time", (object?)Journey.start_time ?? DBNull.Value);
                        command.Parameters.AddWithValue("@end_time", (object?)Journey.end_time ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status", Journey.status ?? "");
                        command.Parameters.AddWithValue("@cost", (object?)Journey.cost ?? DBNull.Value);
                        command.Parameters.AddWithValue("@pickup_location", Journey.pickup_location ?? "");
                        command.Parameters.AddWithValue("@dropoff_location", Journey.dropoff_location ?? "");

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Journey updated successfully!";
                return RedirectToPage("/Journey/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
                return Page();
            }
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
            public DateTime created_at { get; set; }
            public string? pickup_location { get; set; }
            public string? dropoff_location { get; set; }
        }
    }
}
