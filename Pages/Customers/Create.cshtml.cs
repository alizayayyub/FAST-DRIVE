using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Customers
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CustomerInput Customer { get; set; } = new();

        public string Message = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Customer (name, email, phone, avg_rating) VALUES (@name, @email, @phone, @avg_rating)";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", Customer.Name);
                        cmd.Parameters.AddWithValue("@email", Customer.Email);
                        cmd.Parameters.AddWithValue("@phone", Customer.Phone);
                        cmd.Parameters.AddWithValue("@avg_rating", string.IsNullOrEmpty(Customer.AvgRating) ? 0 : double.Parse(Customer.AvgRating));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Customers/Index");
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
                return Page();
            }
        }

        public class CustomerInput
        {
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string AvgRating { get; set; } = "";
        }
    }
}
