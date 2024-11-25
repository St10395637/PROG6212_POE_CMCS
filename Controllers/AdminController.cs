using Claiming_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Document = iTextSharp.text.Document;
using System.Data; // Alias to resolve ambiguity

namespace CMCS.Controllers
{
    public class AdminController : Controller
    {
        private string connectionString = "server=localhost;database=claimsystem;uid=root;password=;"; // Database connection string

        // Action to show Admin registration form (GET request)
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminRegister()
        {
            return View(new AdminRegisterViewModel());  // Initialize and return the empty view model for registration
        }

        // Action to handle form submission for Admin registration (POST request)
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AdminRegister(AdminRegisterViewModel admin)
        {
            if (ModelState.IsValid) // Check if the submitted model is valid
            {
                try
                {
                    // Check if required fields are empty
                    if (string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password) || string.IsNullOrEmpty(admin.Name))
                    {
                        ModelState.AddModelError("", "All fields are required."); // Add model error if fields are empty
                        return View(admin);
                    }

                    // Database connection to insert the new admin user
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open(); // Open database connection

                        // Insert the user into the User table with plain text password
                        var cmdUser = new MySqlCommand("INSERT INTO user (email, password) VALUES (@Email, @Password); SELECT LAST_INSERT_ID();", conn);
                        cmdUser.Parameters.AddWithValue("@Email", admin.Email);
                        cmdUser.Parameters.AddWithValue("@Password", admin.Password); // Plain text password (use hashing in production)

                        // Get the UserId of the newly inserted user
                        var userId = Convert.ToInt32(cmdUser.ExecuteScalar());

                        // Insert into the Admin table using the UserId (foreign key) and Role
                        var cmdAdmin = new MySqlCommand("INSERT INTO admin (id, role) VALUES (@UserId, @Role)", conn);
                        cmdAdmin.Parameters.AddWithValue("@UserId", userId);  // Foreign key reference to User table
                        cmdAdmin.Parameters.AddWithValue("@Role", admin.Role); // Role from the view model

                        int result = cmdAdmin.ExecuteNonQuery(); // Execute the insert query for the admin

                        if (result > 0)
                        {
                            return RedirectToAction("Login"); // Redirect to Login page on success
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to register admin."); // Add error if registration fails
                            return View(admin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or show error message
                    ModelState.AddModelError("", $"Error occurred: {ex.Message}");
                    return View(admin);
                }
            }

            return View(admin); // Return the registration view if model is not valid
        }

        // Action to show Admin login form (GET request)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            return View(); // Return the login view
        }

        // Action to handle admin login submission (POST request)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            if (ModelState.IsValid) // Check if the model is valid
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync(); // Open database connection asynchronously
                    var query = "SELECT * FROM User WHERE Email = @Email"; // Query to check the email in the User table

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", model.Email); // Add parameter for email

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows) // Check if email exists in the database
                            {
                                while (await reader.ReadAsync())
                                {
                                    var storedPassword = reader["Password"].ToString(); // Fetch stored password
                                    if (storedPassword == model.Password) // Compare with entered password
                                    {
                                        return RedirectToAction("ManageClaims", "Admin"); // Redirect to ManageClaims if successful
                                    }
                                }
                            }
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt."); // Add error if login fails
            }

            return View(model); // Return login view if model is not valid
        }

        // Action to display and manage claims (GET request)
        [HttpGet]
        public async Task<IActionResult> ManageClaims()
        {
            var claims = new List<ClaimViewModel>(); // Initialize an empty list to hold claim data

            // Fetch claims data from the database
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync(); // Open connection to the database

                var query = @"SELECT 
                        c.id AS ClaimID,
                        c.hours_worked AS HoursWorked,
                        c.hourly_rate AS HourlyRate,
                        c.total_claim AS TotalClaim,
                        c.status AS ClaimStatus,
                        c.month AS Month, -- Ensure the alias matches the property name
                        c.file_name AS SupportingDocumentName,
                        l.name AS LecturerName,
                        l.department AS LecturerDepartment
                      FROM claims c
                      JOIN lecturer l ON c.lecturer_id = l.id"; // SQL query to join claims and lecturer tables

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) // Loop through the results
                    {
                        claims.Add(new ClaimViewModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ClaimID")),
                            LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                            LecturerDepartment = reader.GetString(reader.GetOrdinal("LecturerDepartment")),
                            Month = reader.GetString(reader.GetOrdinal("Month")),
                            HoursWorked = reader.GetDecimal(reader.GetOrdinal("HoursWorked")),
                            HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                            TotalClaim = reader.GetDecimal(reader.GetOrdinal("TotalClaim")),
                            Status = reader.GetString(reader.GetOrdinal("ClaimStatus")),
                            SupportingDocument = reader.IsDBNull(reader.GetOrdinal("SupportingDocumentName"))
                                                 ? null
                                                 : Path.Combine("/uploads", reader.GetString(reader.GetOrdinal("SupportingDocumentName")))
                        });
                    }
                }
            }

            return View(claims); // Return the view with the claims data
        }

        // Action to update the status of a claim (POST request)
        [HttpPost]
        public async Task<IActionResult> UpdateClaimStatus(int claimId, string action)
        {
            string newStatus = action == "approve" ? "Approved" : "Rejected"; // Determine the new status based on action

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync(); // Open database connection
                var query = "UPDATE claims SET status = @Status WHERE id = @ClaimID"; // SQL query to update claim status
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", newStatus);
                    command.Parameters.AddWithValue("@ClaimID", claimId);
                    await command.ExecuteNonQueryAsync(); // Execute the update query
                }
            }

            return RedirectToAction(nameof(ManageClaims)); // Redirect back to ManageClaims view
        }

        // Action to approve a claim (POST request)
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync(); // Open database connection
                var query = "UPDATE claims SET status = 'Approved' WHERE id = @claimId"; // SQL query to approve claim
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@claimId", claimId);
                    await command.ExecuteNonQueryAsync(); // Execute the approval query
                }
            }
            TempData["SuccessMessage"] = "Claim approved successfully."; // Set success message
            return RedirectToAction("ManageClaims"); // Redirect back to ManageClaims view
        }

        // Action to reject a claim (POST request)
        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync(); // Open database connection
                var query = "UPDATE claims SET status = 'Rejected' WHERE id = @claimId"; // SQL query to reject claim
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@claimId", claimId);
                    await command.ExecuteNonQueryAsync(); // Execute the rejection query
                }
            }
            TempData["SuccessMessage"] = "Claim rejected successfully."; // Set success message
            return RedirectToAction("ManageClaims"); // Redirect back to ManageClaims view
        }

        // Action to generate and download the claims report (GET request)
        [HttpGet]
        public async Task<IActionResult> GenerateReport()
        {
            var
