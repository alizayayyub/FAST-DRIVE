// Pages/Vehicle/Vehicle.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace FAST_DRIVE.Pages.Vehicle
{
    public class VehicleModel : PageModel
    {
        private readonly ILogger<VehicleModel> _logger;

        public VehicleModel(ILogger<VehicleModel> logger)
        {
            _logger = logger;
        }

        public List<VehicleInfo> VehiclesList { get; set; } = new();

        public void OnGet()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT vehicle_id, driver_id, registration_number, model, color, capacity, created_at FROM Vehicle ORDER BY vehicle_id DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VehicleInfo vehicle = new VehicleInfo
                                {
                                    VehicleId = reader.GetInt32(0),
                                    DriverId = reader.GetInt32(1),
                                    RegistrationNumber = reader.GetString(2),
                                    Model = reader.GetString(3),
                                    Color = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    Capacity = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    CreatedAt = reader.GetDateTime(6)
                                };

                                VehiclesList.Add(vehicle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving vehicle data: {Message}", ex.Message);
            }
        }

        public class VehicleInfo
        {
            public int VehicleId { get; set; }
            public int DriverId { get; set; }
            public string RegistrationNumber { get; set; } = "";
            public string Model { get; set; } = "";
            public string Color { get; set; } = "";
            public int Capacity { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
