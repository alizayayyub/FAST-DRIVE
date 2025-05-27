using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Payment
{
    public class PaymentsModel : PageModel
    {
        public List<PaymentInfo> PaymentsList { get; set; } = new();

        public void OnGet()
        {
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"SELECT payment_id, journey_id, amount, status, timestamp 
                                   FROM Payment ORDER BY payment_id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PaymentsList.Add(new PaymentInfo
                                {
                                    payment_id = reader.GetInt32(0),
                                    journey_id = reader.GetInt32(1),
                                    amount = reader.GetDecimal(2).ToString("0.00"),
                                    status = reader.GetString(3),
                                    timestamp = reader.GetDateTime(4).ToString("dd/MM/yyyy HH:mm")
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

        public class PaymentInfo
        {
            public int payment_id { get; set; }
            public int journey_id { get; set; }
            public string amount { get; set; } = "";
            public string status { get; set; } = "";
            public string timestamp { get; set; } = "";
        }
    }
}
