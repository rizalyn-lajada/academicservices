using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class RegisterModel : PageModel
{
    [BindProperty]
    public UserInput Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", Input.Name);
            command.Parameters.AddWithValue("@Email", Input.Email);
            command.Parameters.AddWithValue("@Password", Input.Password);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        return RedirectToPage("Success"); // Add a success page later
    }

    public class UserInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
