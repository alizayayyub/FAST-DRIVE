using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace FAST_DRIVE.Pages.Vehicle
{
    public class EditModel : PageModel
    {
        [BindProperty] public int VehicleId { get; set; }
        [BindProperty] public int DriverId { get; set; }
        [BindProperty] public string RegistrationNumber { get; set; } = "";
        [BindProperty] public string Model { get; set; } = "";
        [BindProperty] public string? Color { get; set; }
        [BindProperty] public int? Capacity { get; set; }

        public void OnGet(int id)
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string sql = "SELECT * FROM Vehicle WHERE vehicle_id = @id";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                VehicleId = reader.GetInt32(0);
                DriverId = reader.GetInt32(1);
                RegistrationNumber = reader.GetString(2);
                Model = reader.GetString(3);
                Color = reader.IsDBNull(4) ? null : reader.GetString(4);
                Capacity = reader.IsDBNull(5) ? null : reader.GetInt32(5);
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string sql = @"UPDATE Vehicle 
                           SET driver_id = @driver_id, registration_number = @reg, model = @model, color = @color, capacity = @capacity 
                           WHERE vehicle_id = @id";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@driver_id", DriverId);
            cmd.Parameters.AddWithValue("@reg", RegistrationNumber);
            cmd.Parameters.AddWithValue("@model", Model);
            cmd.Parameters.AddWithValue("@color", (object?)Color ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@capacity", (object?)Capacity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", VehicleId);

            cmd.ExecuteNonQuery();

            return RedirectToPage("/Vehicle/Vehicle");
        }
    }
}
