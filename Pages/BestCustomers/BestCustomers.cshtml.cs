using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace FAST_DRIVE.Pages.BestCustomers
{
    public class BestCustomersModel : PageModel
    {
        public List<CustomerInfo> CustomersList { get; set; } = new();

        public void OnGet()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT TOP 5 
                        c.customer_id,
                        c.name,
                        c.email,
                        COUNT(DISTINCT j.journey_id) as rides_last_3_months,
                        ROUND(SUM(j.cost) / 3, 2) as avg_monthly_spend,
                        SUM(CASE WHEN r.score = 5 AND r.rater_type = 'DRIVER' THEN 1 ELSE 0 END) as five_star_ratings
                    FROM Customer c
                    JOIN Journey j ON c.customer_id = j.customer_id
                    LEFT JOIN Rating r ON j.journey_id = r.journey_id
                    WHERE j.start_time >= DATEADD(MONTH, -3, GETDATE()) AND j.status = 'COMPLETED'
                    GROUP BY c.customer_id, c.name, c.email
                    HAVING 
                        COUNT(DISTINCT j.journey_id) >= 10
                        OR ROUND(SUM(j.cost) / 3, 2) > 100
                        OR SUM(CASE WHEN r.score = 5 AND r.rater_type = 'DRIVER' THEN 1 ELSE 0 END) >= 5
                    ORDER BY rides_last_3_months DESC, avg_monthly_spend DESC;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomersList.Add(new CustomerInfo
                        {
                            CustomerId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            RidesLast3Months = reader.GetInt32(3),
                            AvgMonthlySpend = reader.GetDecimal(4).ToString("0.00"),
                            FiveStarRatings = reader.GetInt32(5)
                        });
                    }
                }
            }
        }

        public class CustomerInfo
        {
            public int CustomerId { get; set; }
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public int RidesLast3Months { get; set; }
            public string AvgMonthlySpend { get; set; } = "";
            public int FiveStarRatings { get; set; }
        }
    }
}
