using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Misc
{
    public static class ExceptionHandlerExtensions
    {
        public static WebApplication ConfigureGlobalExceptionHandling(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    var problemDetails = new ValidationProblemDetails();

                    if (exception is ValidationException validationException)
                    {
                        problemDetails.Title = "One or more validation errors occurred.";
                        problemDetails.Status = (int)HttpStatusCode.BadRequest;
                        appendValidationErrors(problemDetails, validationException);
                    }
                    else if (exception is UnauthorizedAccessException)
                    {
                        problemDetails.Title = "Authorization error.";
                        problemDetails.Status = (int)HttpStatusCode.Forbidden;
                        problemDetails.Detail = exception.Message;
                    }
                    else if (exception is KeyNotFoundException)
                    {
                        problemDetails.Title = "Resource not found.";
                        problemDetails.Status = (int)HttpStatusCode.NotFound;
                        appendData(problemDetails, exception);
                    }
                    else
                    {
                        problemDetails.Title = "An unexpected error occurred.";
                        problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                        problemDetails.Detail = exception!.Message;
                    }

                    // Unique ID for tracking purposes
                    problemDetails.Instance = $"urn:uuid:{Guid.NewGuid()}";

                    context.Response.ContentType = "application/problem+json";
                    context.Response.StatusCode = problemDetails.Status!.Value;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                });
            });

            return app;
        }

        private static void appendData(ValidationProblemDetails problemDetails, Exception exception)
        {
            problemDetails.Detail = exception.Message;
            foreach (var key in exception.Data.Keys)
            {
                var value = exception.Data[key];
                if (value is string errorMessage)
                {
                    problemDetails.Errors.Add(key.ToString()!, new[] { errorMessage });
                }
            }
        }

        private static void appendValidationErrors(ValidationProblemDetails problemDetails, Exception exception)
        {
            // Set the main error message as the problem detail
            problemDetails.Detail = exception.Message;

            foreach (var key in exception.Data.Keys)
            {
                string errorCode = key.ToString()!;
                var errorMessage = exception.Data[key]?.ToString() ?? string.Empty;

                // Map Identity framework error codes to standard property names if applicable
                string mappedKey = errorCode switch
                {
                    "DuplicateUserName" => nameof(RegisterUserDto.Name),
                    "DuplicateEmail" => nameof(RegisterUserDto.Email),
                    "PasswordTooShort" => nameof(RegisterUserDto.Password),
                    "PasswordRequiresDigit" => nameof(RegisterUserDto.Password),
                    "PasswordRequiresUpper" => nameof(RegisterUserDto.Password),
                    "PasswordRequiresLower" => nameof(RegisterUserDto.Password),
                    "PasswordRequiresNonAlphanumeric" => nameof(RegisterUserDto.Password),
                    "PasswordRequiresUniqueChars" => nameof(RegisterUserDto.Password),
                    _ => errorCode // Use the original key if it doesn't match known Identity codes
                };

                // If the mapped key already exists, add the error message to the existing array
                if (problemDetails.Errors.ContainsKey(mappedKey!))
                {
                    var existingErrors = problemDetails.Errors[mappedKey!].ToList(); // Convert to list for easy manipulation
                    existingErrors.Add(errorMessage);
                    problemDetails.Errors[mappedKey!] = existingErrors.ToArray(); // Convert back to array and assign
                }
                else
                {
                    // Initialize with a single-element array if the key doesn't exist
                    problemDetails.Errors[mappedKey!] = new[] { errorMessage };
                }
            }
        }


    }
}
