using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FAST_DRIVE.Pages.Ratings
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public RatingInput Rating { get; set; } = new();

        public string Message = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                string connStr = @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;";
                using SqlConnection conn = new(connStr);
                conn.Open();

                string sql = @"INSERT INTO Rating (journey_id, rater_id, rater_type, score, comment)
                               VALUES (@journey_id, @rater_id, @rater_type, @score, @comment)";

                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@journey_id", Rating.JourneyId);
                cmd.Parameters.AddWithValue("@rater_id", Rating.RaterId);
                cmd.Parameters.AddWithValue("@rater_type", Rating.RaterType);
                cmd.Parameters.AddWithValue("@score", Rating.Score);
                cmd.Parameters.AddWithValue("@comment", Rating.Comment ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();

                return RedirectToPage("/Ratings/Ratings");
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
                return Page();
            }
        }

        public class RatingInput
        {
            public int JourneyId { get; set; }
            public int RaterId { get; set; }
            public string RaterType { get; set; } = "";
            public double Score { get; set; }
            public string? Comment { get; set; }
        }
    }
}
