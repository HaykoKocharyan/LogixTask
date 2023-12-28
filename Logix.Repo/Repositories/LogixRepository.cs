using Logix.Repo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logix.Repo.Repositories
{
    public class LogixRepository
    {
        private readonly LogixDBContext dBContext;
        public LogixRepository(LogixDBContext dBContext)
        {
            this.dBContext = new LogixDBContext();
        }
        public LogixRepository()
        {

        }

        public async Task Register(Dictionary<string, object> userData)
        {
            var username = userData["Username"]?.ToString();
            var existingUsername = await dBContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUsername != null)
            {
                throw new InvalidOperationException("User with the same username already exists");
            }

            var existingEmail = userData["Email"]?.ToString();
            var existingUserByEmail = await dBContext.Users.FirstOrDefaultAsync(u => u.Email == existingEmail);
            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException("User with the same email already exists");
            }

            var user = new User
            {
                FirstName = userData["FirstName"].ToString(),
                LastName = userData["LastName"].ToString(),
                //FullName = $"{userData["FirstName"]} {userData["LastName"]}",
                DateOfBirth = (DateTime)userData["DateOfBirth"],
                Email = userData["Email"].ToString(),
                PhoneNumber = userData["PhoneNumber"].ToString(),
                Address = userData["Address"].ToString(),
                Username = userData["Username"].ToString(),
                Password = userData["Password"].ToString()
            };

            dBContext.Users.Add(user);
            await dBContext.SaveChangesAsync();
        }

        public async Task Login(List<string> loginpass)
        {
            var existingUser = await dBContext.Users.FirstOrDefaultAsync(s => s.Username == loginpass[0] || s.Email == loginpass[0]);
            if (existingUser != null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginpass[1], existingUser.Password);
                if (isPasswordValid)
                {

                }
                else
                {
                    throw new InvalidOperationException("Incorrect Password");
                }
            }
            else
            {
                throw new InvalidOperationException("Incorrect Username or Password");
            }
        }

        public async Task<User> GetUserInfo(int userId)
        {
            var existingUser = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser != null)
            {
                return existingUser;
            }

            else throw new InvalidOperationException("User not found");
        }

        public async Task CreateClass(List<dynamic> classlist)
        {
            Class newClass = new Class
            {
                Name = classlist[0]
            };

            dBContext.Classes.Add(newClass);
            await dBContext.SaveChangesAsync();
        }

        public async Task AddClassToUser(List<int> ints)
        {
            var userclass = new UserClass
            {
                ClassId = ints[0],
                UserId = ints[1]
            };

            dBContext.UserClasses.Add(userclass);
            await dBContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId, string password)
        {
            var existingUser = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser != null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, existingUser.Password);
                if (isPasswordValid)
                {
                    dBContext.Users.Remove(existingUser);
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new InvalidOperationException("Incorrect password");
            }
            else
                throw new InvalidOperationException("User not found");
        }

        public async Task EditUser(int userId, string password, Dictionary<string, object?> userDictionary)
        {
            var existingUser = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (existingUser != null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, existingUser.Password);
                if (isPasswordValid)
                {
                    foreach (var kvp in userDictionary)
                    {
                        if (kvp.Value != null)
                        {
                            var property = existingUser.GetType().GetProperty(kvp.Key);
                            if (property != null)
                            {
                                property.SetValue(existingUser, kvp.Value);
                            }
                        }
                    }
                    await dBContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Incorrect password");
                }
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task DeleteClassForUser(int userId, string password, int classId)
        {
            var existingUser = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser != null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, existingUser.Password);
                if (isPasswordValid)
                {
                    var existingUserClass = await dBContext.UserClasses.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClassId == classId);
                    if (existingUserClass != null)
                    {
                        dBContext.UserClasses.Remove(existingUserClass);
                        await dBContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new InvalidOperationException("User is not enrolled in this class");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Incorrect password");
                }
            }
            else
            {
                throw new InvalidOperationException("User not found");
            }
        }
    }
}


