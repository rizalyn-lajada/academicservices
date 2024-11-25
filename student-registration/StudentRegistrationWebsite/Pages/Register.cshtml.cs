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
                string query = @"INSERT INTO Students (FirstName, LastName, Email, DateOfBirth) 
                                 VALUES (@FirstName, @LastName, @Email, @DateOfBirth)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", Input.FirstName);
                command.Parameters.AddWithValue("@LastName", Input.LastName);
                command.Parameters.AddWithValue("@Email", Input.Email);
                command.Parameters.AddWithValue("@DateOfBirth", Input.DateOfBirth);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        return RedirectToPage("Success");
    }

    public class UserInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
