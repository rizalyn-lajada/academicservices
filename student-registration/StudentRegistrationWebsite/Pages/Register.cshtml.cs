public async Task<IActionResult> OnPostAsync()
{
    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

    // Debugging logic: log the connection string
    Console.WriteLine($"Connection string: {connectionString}");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        ModelState.AddModelError(string.Empty, "Database connection is not configured.");
        return Page();
    }

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            string query = "INSERT INTO Students (FirstName, LastName, Email, DateOfBirth) VALUES (@FirstName, @LastName, @Email, @DateOfBirth)";
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
