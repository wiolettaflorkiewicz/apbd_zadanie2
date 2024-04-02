using System;

namespace LegacyApp.Interfaces;

public interface IInputValidator
{
    bool ValidateName(string name);
    bool ValidateEmail(string email);
    bool ValidateAge(DateTime dateOfBirth);
}