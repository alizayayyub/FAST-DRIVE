using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Payment
{
    public class DeleteModel : PageModel
    {
        [BindProperty] public int payment_id { get; set; }
        public int journey_id { get; set; }
        public string amount { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "SELECT journey_id, amount FROM Payment WHERE payment_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    journey_id = reader.GetInt32(0);
                    amount = reader.GetDecimal(1).ToString("0.00");
                    payment_id = id;
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
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "DELETE FROM Payment WHERE payment_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", payment_id);
                cmd.ExecuteNonQuery();

                return RedirectToPage("/Payment/Payment");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }
    }
}
