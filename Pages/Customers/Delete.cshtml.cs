using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        [BindProperty] public int customer_id { get; set; }
        public string name { get; set; } = "";
        public string email { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "SELECT name, email FROM Customer WHERE customer_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    name = reader.GetString(0);
                    email = reader.GetString(1);
                    customer_id = id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "DELETE FROM Customer WHERE customer_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", customer_id);
                cmd.ExecuteNonQuery();

                return RedirectToPage("/Customers/Customers");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }
    }
}
