using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class RegisterModel : PageModel
{
    [BindProperty]
    public UserInput Input { get; set; } = new UserInput();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            ModelState.AddModelError(string.Empty, "Database connection is not configured.");
            return Page();
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", Input.Name);
                command.Parameters.AddWithValue("@Email", Input.Email);
                command.Parameters.AddWithValue("@Password", Input.Password);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        return RedirectToPage("Success"); // Add a success page later
    }

    public class UserInput
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
