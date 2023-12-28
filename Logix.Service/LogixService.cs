using Logix.Models;
using Logix.Repo.Repositories;
using System.Text.RegularExpressions;

namespace Logix.Service
{
    public class LogixService
    {
        private readonly LogixRepository logixRepository;

        public LogixService(LogixRepository logixRepository)
        {
            this.logixRepository = logixRepository;
        }

        public async Task Register(UserRegisterModel userRegisterModel)
        {
            string digitsOnly = Regex.Replace(userRegisterModel.PhoneNumber, @"[^\d]", "");
            string formattedPhoneNumber = Regex.Replace(digitsOnly, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");

            string address = userRegisterModel.Address.Replace(".", " ");
            string[] words = address.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i].ToLower();
                if (word == "apartment")
                    words[i] = "APT";
                if (word == "avenue" || word == "ave" || word == "avenue no" || word == "avenue no.")
                    words[i] = "AVE";
                if (word == "road")
                    words[i] = "RD";
                if (word == "street")
                    words[i] = "ST";
                if (word == "boulevard")
                    words[i] = "BLVD";

                words[i] = words[i].ToUpper();
            }

            string formattedAddress = string.Join(" ", words);

            userRegisterModel.Address = formattedAddress;
            userRegisterModel.PhoneNumber = formattedPhoneNumber;
            userRegisterModel.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterModel.Password);

            var userDictionary = userRegisterModel.GetType()
                .GetProperties()
                .Where(prop => prop.GetValue(userRegisterModel) != null)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(userRegisterModel));

            await logixRepository.Register(userDictionary);
        }

        public async Task Login(UserLoginModel model)
        {
            List<string> loginpass = new List<string>
            {
            model.UsernameOrEmail,
            model.Password
            };

            await logixRepository.Login(loginpass);
        }

        public async Task<dynamic> GetUserInfo(int userId)
        {
            return await logixRepository.GetUserInfo(userId);
        }

        public async Task CreateClass(ClassModel classModel)
        {
            List<dynamic> classlist = new List<dynamic>
            {
                classModel.Name
            };
            await logixRepository.CreateClass(classlist);
        }

        public async Task AddClassToUser(AddClassToUser addClassToUser)
        {
            List<int> ints = new List<int>
            {
                addClassToUser.ClassId,
                addClassToUser.UserId
            };

            await logixRepository.AddClassToUser(ints);
        }

        public async Task DeleteUser(int userId, string password)
        {
            await logixRepository.DeleteUser(userId, password);
        }

        public async Task EditUser(int userId, string password, EditUserModel editUserModel)
        {
            if (editUserModel.PhoneNumber != null)
            {
                string digitsOnly = Regex.Replace(editUserModel.PhoneNumber, @"[^\d]", "");
                string formattedPhoneNumber = Regex.Replace(digitsOnly, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");

                editUserModel.PhoneNumber = formattedPhoneNumber;
            }
            var email = editUserModel.Email;

            if (!string.IsNullOrEmpty(email))
            {
                bool isEmailValid = Regex.IsMatch(email,
                    @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

                if (!isEmailValid)
                {
                    throw new InvalidOperationException("Invalid email address format");
                }
            }
            if (editUserModel.Address != null)
            {
                string address = editUserModel.Address.Replace(".", " ");

                string[] words = editUserModel.Address.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i].ToLower();
                    if (word == "apartment")
                        words[i] = "APT";
                    if (word == "avenue" || word == "ave" || word == "avenue no" || word == "avenue no.")
                        words[i] = "AVE";
                    if (word == "road")
                        words[i] = "RD";
                    if (word == "street")
                        words[i] = "ST";
                    if (word == "boulevard")
                        words[i] = "BLVD";

                    words[i] = words[i].ToUpper();
                }

                string formattedAddress = string.Join(" ", words);

                editUserModel.Address = formattedAddress;
            }
            if (!string.IsNullOrEmpty(editUserModel.NewPassword))
            {
                editUserModel.NewPassword = BCrypt.Net.BCrypt.HashPassword(editUserModel.NewPassword);
            }

            var nonNullProperties = editUserModel.GetType()
                 .GetProperties()
                 .Where(prop => prop.GetValue(editUserModel) != null);

            var userDictionary = nonNullProperties.ToDictionary(
                prop => prop.Name == "NewPassword" ? "Password" : prop.Name,
                prop => prop.GetValue(editUserModel));

            await logixRepository.EditUser(userId, password, userDictionary);
        }

        public async Task DeleteClassForUser(int userId, string password, int classId)
        {
            await logixRepository.DeleteClassForUser(userId, password, classId);
        }
    }
}

