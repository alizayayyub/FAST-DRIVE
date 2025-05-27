using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Drivers
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public DriverInfo Driver { get; set; } = new();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Driver WHERE driver_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Driver.Id = reader.GetInt32(0);
                                Driver.Name = reader.GetString(1);
                                Driver.Email = reader.GetString(2);
                                Driver.Phone = reader.GetString(3);
                                Driver.LicenseNumber = reader.GetString(4);
                                Driver.AvgRating = reader.GetDouble(5).ToString("0.0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
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
                    string sql = @"UPDATE Driver 
                                   SET name=@name, email=@email, phone=@phone, license_number=@license_number, avg_rating=@avg_rating 
                                   WHERE driver_id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", Driver.Name);
                        command.Parameters.AddWithValue("@email", Driver.Email);
                        command.Parameters.AddWithValue("@phone", Driver.Phone);
                        command.Parameters.AddWithValue("@license_number", Driver.LicenseNumber);
                        command.Parameters.AddWithValue("@avg_rating", Driver.AvgRating);
                        command.Parameters.AddWithValue("@id", Driver.Id);

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Driver updated successfully!";
                return RedirectToPage("/Drivers/Driver");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        public class DriverInfo
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; } = "";

            [Required, EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            public string Phone { get; set; } = "";

            [Required]
            public string LicenseNumber { get; set; } = "";

            [Required]
            public string AvgRating { get; set; } = "";
        }
    }
}
