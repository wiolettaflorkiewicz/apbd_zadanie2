using System;
using LegacyApp.Interfaces;

namespace LegacyApp.Validators.Users;

public class InputValidator : IInputValidator
{
    public bool ValidateName(string name)
    {
        return !string.IsNullOrEmpty(name);
    }

    public bool ValidateEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public bool ValidateAge(DateTime dateOfBirth)
    {
        var now = DateTime.Now;
        int age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
        {
            age--;
        }
        return age >= 21;
    }
}