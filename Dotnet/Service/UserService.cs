using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Data.Interface;
using Microsoft.Extensions.Options;
using Model;
using Model.Entities;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helper;
using Service.Interface;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly byte[] Key;
        private readonly byte[] IV;

        public UserService(IUserRepository userRepository, IMapper mapper, IOptions<Encryption> encryptionSettings)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            Key = Encoding.UTF8.GetBytes(encryptionSettings.Value.Key);
            IV = Encoding.UTF8.GetBytes(encryptionSettings.Value.IV);
        }

        public async Task<ApiResponse> Create(CreateUserRequest dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Phone))
                {
                    return new ApiResponse().BadRequestResponse("Name, Email, Password and Phone are required");
                }

                var doesUserEmailExists = await userRepository.GetByEmail(dto.Email);
                if (doesUserEmailExists != null)
                {
                    return new ApiResponse().BadRequestResponse("Email already exists");
                }

                var user = new Users(dto.Name, dto.Email, EncryptPassword(dto.Password), dto.Phone, (int)UserRole.User);

                var createdUser = await userRepository.Create(user);
                if (createdUser == null)
                {
                    return new ApiResponse().BadRequestResponse("Failed to create user");
                }
                return new ApiResponse().CreatedResponse(mapper.Map<UserResponse>(await userRepository.GetByEmail(dto.Email)));
            }
            catch (Exception ex)
            {
                return new ApiResponse().InternalServerErrorResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> Login(LoginRequest dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.EmailOrPhone) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    return new ApiResponse().BadRequestResponse("Email or Phone and Password are required");
                }

                var user = await userRepository.GetByEmailOrPhoneAndPassword(dto.EmailOrPhone, EncryptPassword(dto.Password));
                if (user == null)
                {
                    return new ApiResponse().BadRequestResponse("Invalid credentials");
                }
                return new ApiResponse().SucessResponse(mapper.Map<UserResponse>(user));
            }
            catch (Exception ex)
            {
                return new ApiResponse().InternalServerErrorResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetUserProfile(string email)
        {
            try
            {
                var user = await userRepository.GetByEmail(email);
                if (user == null)
                {
                    return new ApiResponse().NotFoundResponse("User not found");
                }
                return new ApiResponse().SucessResponse(mapper.Map<UserResponse>(user));
            }
            catch (Exception ex)
            {
                return new ApiResponse().InternalServerErrorResponse(ex.Message);
            }
        }

        // Encrypts a plain text password
        public string EncryptPassword(string plainPassword)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainPassword);
                    swEncrypt.Close();
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Decrypts an encrypted password
        public string DecryptPassword(string encryptedPassword)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}