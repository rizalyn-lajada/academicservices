using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;

public class StudentModelPage : PageModel
{
    public List<StudentModel> Students { get; set; } = new List<StudentModel>();

    public void OnGet()
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT StudentID, FirstName, LastName, Email, DateOfBirth FROM Students";
            SqlCommand command = new SqlCommand(query, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Students.Add(new StudentModel
                    {
                        StudentID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        DateOfBirth = reader.GetDateTime(4)
                    });
                }
            }
        }
    }
}

public class StudentModel
{
    public int StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
}
