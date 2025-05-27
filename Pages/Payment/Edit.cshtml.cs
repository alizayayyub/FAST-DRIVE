using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Payment
{
    public class EditModel : PageModel
    {
        [BindProperty] public int payment_id { get; set; }
        [BindProperty] public int journey_id { get; set; }
        [BindProperty] public string amount { get; set; } = "";
        [BindProperty] public string status { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "SELECT * FROM Payment WHERE payment_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    payment_id = reader.GetInt32(0);
                    journey_id = reader.GetInt32(1);
                    amount = reader.GetDecimal(2).ToString("0.00");
                    status = reader.GetString(3);
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
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = @"UPDATE Payment SET 
                                journey_id = @journey_id,
                                amount = @amount,
                                status = @status
                               WHERE payment_id = @id";

                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@journey_id", journey_id);
                cmd.Parameters.AddWithValue("@amount", decimal.Parse(amount));
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", payment_id);

                cmd.ExecuteNonQuery();
                return RedirectToPage("/Payment/Payment");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
