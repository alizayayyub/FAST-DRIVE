using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Drivers
{
    public class DriverModel : PageModel
    {
        public List<DriverInfo> DriversList { get; set; } = new();

        public void OnGet()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT driver_id, name, email, phone, license_number, avg_rating, created_at FROM Driver ORDER BY driver_id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DriversList.Add(new DriverInfo
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.GetString(3),
                                    LicenseNumber = reader.GetString(4),
                                    AvgRating = reader.GetDouble(5).ToString("0.0"),
                                    CreatedAt = reader.GetDateTime(6).ToString("dd/MM/yyyy")
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

        public class DriverInfo
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string LicenseNumber { get; set; } = "";
            public string AvgRating { get; set; } = "";
            public string CreatedAt { get; set; } = "";
        }
    }
}
