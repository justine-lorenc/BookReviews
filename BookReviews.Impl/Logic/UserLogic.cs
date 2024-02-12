using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories.Interfaces;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic
{
    public class UserLogic : IUserLogic
    {
        private IMapper _mapper;
        private IUserRepository _userRepository;

        public UserLogic(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Models.User> AuthenticateUser(string username, string password)
        {
            try
            {
                Entities.User userRecord = await _userRepository.GetUser(username);

                if (userRecord == null || !userRecord.IsActive)
                    throw new Exception("Failed to retrieve user");

                bool credentialsValid = VerifyCredentials(username, password, userRecord.PasswordHash);

                if (!credentialsValid)
                    throw new Exception("Invalid credentials");

                Models.User userResult = _mapper.Map<Models.User>(userRecord);
                return userResult;
            }
            catch (Exception ex)
            {
                // log error here
                return null;
            }
        }

        public async Task<bool> RegisterAccount(Models.User user, string password)
        {
            try
            {
                // check that an account with this email address does not exist
                Entities.User userRecord = await _userRepository.GetUser(user.EmailAddress);

                if (userRecord != null)
                    throw new Exception("An account already exists for this email");

                string hashedPassword = HashPassword(user.EmailAddress, password);

                Entities.User newUserRecord = _mapper.Map<Entities.User>(user);
                newUserRecord.PasswordHash = hashedPassword;

                int userId = await _userRepository.InsertUser(newUserRecord);

                if (userId == 0)
                    throw new Exception("Failed to register account");

                return true;
            }
            catch (Exception ex)
            {
                // log error here
                return false;
            }
        }

        public async Task<List<Models.Enums.Role>> GetUserRoles(int userId)
        {
            try
            {
                List<Entities.Enums.Role> roleRecords = await _userRepository.GetUserRoles(userId);
                List<Models.Enums.Role> roleResults = _mapper.Map<List<Models.Enums.Role>>(roleRecords);

                return roleResults;
            }
            catch (Exception ex)
            {
                // log error here
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
