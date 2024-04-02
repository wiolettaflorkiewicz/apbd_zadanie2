using System;
using LegacyApp.Interfaces;
using LegacyApp.Validators.Users;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly IInputValidator _inputValidator;
        public UserService() : this(new ClientRepository(), new UserCreditService(), new InputValidator())
        {
        }
        public UserService(IClientRepository clientRepository, IUserCreditService userCreditService, IInputValidator inputValidator)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
            _inputValidator = inputValidator;
        }
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!_inputValidator.ValidateName(firstName) || !_inputValidator.ValidateName(lastName))
            {
                return false;
            }

            if (!_inputValidator.ValidateEmail(email))
            {
                return false;
            }

            if (!_inputValidator.ValidateAge(dateOfBirth))
            {
                return false;
            }
            
            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client == null)
            {
                throw new ArgumentException("Invalid client ID.");
            }

            AssignCreditLimit(user, client.Type);

            if (!CanAddUser(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
        private bool ValidateInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            return _inputValidator.ValidateName(firstName) &&
                   _inputValidator.ValidateName(lastName) &&
                   _inputValidator.ValidateEmail(email) &&
                   _inputValidator.ValidateAge(dateOfBirth);
        }

        private void AssignCreditLimit(User user, string clientType)
        {
            switch (clientType)
            {
                case "VeryImportantClient":
                    user.HasCreditLimit = false;
                    break;
                case "ImportantClient":
                    int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
                    user.CreditLimit = creditLimit;
                    break;
                default:
                    user.HasCreditLimit = true;
                    user.CreditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    break;
            }
        }

        private bool CanAddUser(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }
    }
}

