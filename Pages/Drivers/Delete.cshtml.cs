using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Drivers
{
    public class DeleteModel : PageModel
    {
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        [BindProperty]
        public DriverInfo Driver { get; set; } = new();

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
                            else
                            {
                                ErrorMessage = "Driver not found.";
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
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Driver WHERE driver_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", Driver.Id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            ErrorMessage = "Delete failed. Driver not found.";
                            return Page();
                        }
                    }
                }

                SuccessMessage = "Driver deleted successfully.";
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
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string LicenseNumber { get; set; } = "";
            public string AvgRating { get; set; } = "";
        }
    }
}
