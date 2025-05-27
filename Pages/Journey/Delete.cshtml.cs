using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Journey
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int journey_id { get; set; }
        public int customer_id { get; set; }
        public int driver_id { get; set; }
        public int vehicle_id { get; set; }
        public string status { get; set; } = "";
        public decimal cost { get; set; }

        public void OnGet(int id)
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "SELECT customer_id, driver_id, vehicle_id, status, cost FROM Journey WHERE journey_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    customer_id = reader.GetInt32(0);
                    driver_id = reader.GetInt32(1);
                    vehicle_id = reader.GetInt32(2);
                    status = reader.GetString(3);
                    cost = reader.GetDecimal(4);
                    journey_id = id;
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

                string sql = "DELETE FROM Journey WHERE journey_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", journey_id);
                cmd.ExecuteNonQuery();

                return RedirectToPage("/Journey/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }
    }
}
