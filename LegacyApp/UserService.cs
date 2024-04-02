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

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;

            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
        
    }
}
