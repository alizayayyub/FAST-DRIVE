using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Drivers
{
    public class CreateModel : PageModel
    {
        [BindProperty, Required, StringLength(100)]
        public string Name { get; set; } = "";

        [BindProperty, Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = "";

        [BindProperty, Required, Phone, StringLength(20)]
        public string Phone { get; set; } = "";

        [BindProperty, Required, StringLength(50)]
        public string LicenseNumber { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        private readonly string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";

        public void OnGet()
        {
            // Nothing to load on GET for create form
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                var sql = @"
                    INSERT INTO Driver (name, email, phone, license_number)
                    VALUES (@name, @email, @phone, @license_number);
                ";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", Name);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@phone", Phone);
                command.Parameters.AddWithValue("@license_number", LicenseNumber);

                command.ExecuteNonQuery();

                return RedirectToPage("/Drivers/Index");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint error
                {
                    ErrorMessage = "Email, Phone, or License Number already exists.";
                }
                else
                {
                    ErrorMessage = "Database error: " + ex.Message;
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Unexpected error: " + ex.Message;
                return Page();
            }
        }
    }
}
