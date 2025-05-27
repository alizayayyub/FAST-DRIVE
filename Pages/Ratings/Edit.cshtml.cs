using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Ratings
{
    public class EditModel : PageModel
    {
        [BindProperty] public int rating_id { get; set; }
        [BindProperty] public int journey_id { get; set; }
        [BindProperty] public int rater_id { get; set; }
        [BindProperty] public string rater_type { get; set; } = "";
        [BindProperty] public double score { get; set; }
        [BindProperty] public string? comment { get; set; }

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = "SELECT journey_id, rater_id, rater_type, score, comment FROM Rating WHERE rating_id = @id";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rating_id = id;
                    journey_id = reader.GetInt32(0);
                    rater_id = reader.GetInt32(1);
                    rater_type = reader.GetString(2);
                    score = reader.GetDouble(3);
                    comment = reader.IsDBNull(4) ? null : reader.GetString(4);
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

                string sql = @"UPDATE Rating SET
                                journey_id = @journey_id,
                                rater_id = @rater_id,
                                rater_type = @rater_type,
                                score = @score,
                                comment = @comment
                            WHERE rating_id = @rating_id";

                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@journey_id", journey_id);
                cmd.Parameters.AddWithValue("@rater_id", rater_id);
                cmd.Parameters.AddWithValue("@rater_type", rater_type);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.Parameters.AddWithValue("@comment", comment ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@rating_id", rating_id);

                cmd.ExecuteNonQuery();
                return RedirectToPage("/Ratings/Ratings");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
