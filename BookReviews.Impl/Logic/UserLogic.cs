using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories.Interfaces;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace BookReviews.Impl.Logic
{
    public class UserLogic : IUserLogic
    {
        private IMapper _mapper;
        private IExceptionLogic _exceptionLogic;
        private IUserRepository _userRepository;

        public UserLogic(IMapper mapper, IExceptionLogic exceptionLogic, IUserRepository userRepository)
        {
            _mapper = mapper;
            _exceptionLogic = exceptionLogic;
            _userRepository = userRepository;
        }

        public async Task<Models.User> AuthenticateUser(string username, string password)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(username))
                    throw new Exception("Username is null or empty");
                else if (String.IsNullOrWhiteSpace(password))
                    throw new Exception("Password is null or empty");
                else if (!Regex.IsMatch(password, Globals.RegularExpressions.Password))
                    throw new Exception("Password has invalid format");

                Entities.User userRecord = await _userRepository.GetUser(username);

                if (userRecord == null || !userRecord.IsActive)
                    throw new Exception("Failed to retrieve user");

                bool credentialsValid = VerifyCredentials(username, password, userRecord.PasswordHash);

                if (!credentialsValid)
                    throw new Exception("Username or password is incorrect");

                Models.User userResult = _mapper.Map<Models.User>(userRecord);
                return userResult;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("Username", username);

                await _exceptionLogic.LogException(ex, "Authenticate user error", arguments);

                return null;
            }
        }

        public async Task<bool> RegisterAccount(Models.NewAccount account)
        {
            try
            {
                if (account == null)
                    throw new Exception("Account is null");
                else if (!account.IsValid(out string errorMessage))
                    throw new Exception(errorMessage);

                // check that an account with this email address does not exist
                Entities.User userRecord = await _userRepository.GetUser(account.EmailAddress);

                if (userRecord != null)
                    throw new Exception("An account already exists for this email");

                string hashedPassword = HashPassword(account.EmailAddress, account.Password);

                Entities.User newUserRecord = _mapper.Map<Entities.User>(account);
                newUserRecord.PasswordHash = hashedPassword;

                int userId = await _userRepository.InsertUser(newUserRecord);

                if (userId == 0)
                    throw new Exception("Failed to register account");

                return true;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                if (account != null)
                {
                    arguments.Add("EmailAddress", account.EmailAddress);
                    arguments.Add("FirstName", account.FirstName);
                    arguments.Add("LastName", account.LastName);
                }

                await _exceptionLogic.LogException(ex, "Register account error", arguments);

                return false;
            }
        }

        public async Task<List<Models.Enums.Role>> GetUserRoles(int userId)
        {
            try
            {
                if (userId == 0)
                    throw new Exception("User ID is invalid");

                List<Entities.Enums.Role> roleRecords = await _userRepository.GetUserRoles(userId);
                List<Models.Enums.Role> roleResults = _mapper.Map<List<Models.Enums.Role>>(roleRecords);

                return roleResults;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("UserId", userId.ToString());

                await _exceptionLogic.LogException(ex, "Get user roles error", arguments);

                return new List<Models.Enums.Role>();
            }
        }

        private bool VerifyCredentials(string username, string password, string truePassword)
        {
            string hashedPassword = HashPassword(username, password);

            if (!hashedPassword.Equals(truePassword, StringComparison.OrdinalIgnoreCase))
                return false;
            else
                return true;
        }

        private string HashPassword(string username, string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes(Globals.AppSettings.HashSalt);
            byte[] usernameBytes = Encoding.UTF8.GetBytes(username);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            string hashedPassword = String.Empty;

            using (var argon2 = new Argon2i(passwordBytes))
            {
                argon2.DegreeOfParallelism = 24;
                argon2.MemorySize = 8192;
                argon2.Iterations = 60;
                argon2.Salt = salt;
                argon2.AssociatedData = usernameBytes;

                byte[] hash = argon2.GetBytes(32);
                hashedPassword = Convert.ToBase64String(hash);
            }

            return hashedPassword;
        }
    }
}
