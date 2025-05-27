using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Vehicle
{
    public class CreateModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Driver ID is required")]
        public int DriverId { get; set; }

        [BindProperty, Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Model is required")]
        public string Model { get; set; } = "";

        [BindProperty]
        public string? Color { get; set; }

        [BindProperty]
        public int? Capacity { get; set; }

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO Vehicle (driver_id, registration_number, model, color, capacity)
                                   VALUES (@driver_id, @registration_number, @model, @color, @capacity)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@driver_id", DriverId);
                        command.Parameters.AddWithValue("@registration_number", RegistrationNumber);
                        command.Parameters.AddWithValue("@model", Model);
                        command.Parameters.AddWithValue("@color", (object?)Color ?? DBNull.Value);
                        command.Parameters.AddWithValue("@capacity", (object?)Capacity ?? DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
                return Page();
            }

            return RedirectToPage("/Vehicle/Vehicle");
        }
    }
}
