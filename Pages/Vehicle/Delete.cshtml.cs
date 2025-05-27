using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Vehicle
{
    public class DeleteModel : PageModel
    {
        [BindProperty] public int VehicleId { get; set; }
        public string RegistrationNumber { get; set; } = "";
        public string Model { get; set; } = "";

        public void OnGet(int id)
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string sql = "SELECT registration_number, model FROM Vehicle WHERE vehicle_id = @id";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                VehicleId = id;
                RegistrationNumber = reader.GetString(0);
                Model = reader.GetString(1);
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string sql = "DELETE FROM Vehicle WHERE vehicle_id = @id";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@id", VehicleId);
            cmd.ExecuteNonQuery();

            return RedirectToPage("/Vehicle/Vehicle");
        }
    }
}
