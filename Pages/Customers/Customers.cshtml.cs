using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Customers
{
    public class CustomersModel : PageModel
    {
        public List<CustomerInfo> CustomersList { get; set; } = new();

        public void OnGet()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT customer_id, name, email, phone, avg_rating, created_at FROM Customer ORDER BY customer_id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomersList.Add(new CustomerInfo
                                {
                                    customer_id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    avg_rating = reader.GetDouble(4).ToString("0.0"),
                                    created_at = reader.GetDateTime(5).ToString("dd/MM/yyyy")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public class CustomerInfo
        {
            public int customer_id { get; set; }
            public string name { get; set; } = "";
            public string email { get; set; } = "";
            public string phone { get; set; } = "";
            public string avg_rating { get; set; } = "";
            public string created_at { get; set; } = "";
        }
    }
}
