using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Customers
{
    public class EditModel : PageModel
    {
        [BindProperty] public int customer_id { get; set; }
        [BindProperty] public string name { get; set; } = "";
        [BindProperty] public string email { get; set; } = "";
        [BindProperty] public string phone { get; set; } = "";
        [BindProperty] public string avg_rating { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "SELECT * FROM Customer WHERE customer_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    customer_id = reader.GetInt32(0);
                    name = reader.GetString(1);
                    email = reader.GetString(2);
                    phone = reader.GetString(3);
                    avg_rating = reader.GetDouble(4).ToString("0.0");
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
                string connStr = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = @"UPDATE Customer SET 
                                name = @name,
                                email = @email,
                                phone = @phone,
                                avg_rating = @avg_rating
                            WHERE customer_id = @id";

                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@avg_rating", avg_rating);
                cmd.Parameters.AddWithValue("@id", customer_id);

                cmd.ExecuteNonQuery();
                return RedirectToPage("/Customers/Customers");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
