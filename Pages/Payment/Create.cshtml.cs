using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Payment
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public PaymentInput Payment { get; set; } = new();

        public string Message = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Payment (journey_id, amount, status) VALUES (@journey_id, @amount, @status)";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@journey_id", Payment.JourneyId);
                        cmd.Parameters.AddWithValue("@amount", decimal.Parse(Payment.Amount));
                        cmd.Parameters.AddWithValue("@status", Payment.Status);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("Payment/Payment");
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
                return Page();
            }
        }

        public class PaymentInput
        {
            public int JourneyId { get; set; }
            public string Amount { get; set; } = "";
            public string Status { get; set; } = "";
        }
    }
}
