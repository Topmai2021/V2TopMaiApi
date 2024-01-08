using Common.Utils.Exceptions;
using NETCore.Encrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using static Common.Utils.Constant.Const;
using static Common.Utils.Enums.Enums;

namespace Common.Utils.Helpers
{
    public static class Helper
    {
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = DateTime.UtcNow;
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }


        /// <summary>
        /// Method to get value claim from JwtToken
        /// </summary>
        /// <param name="authorization"> Request.Headers["Authorization"] </param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static string GetClaimValue(string token, string claim)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string authHeader = token.Replace("Bearer ", "").Replace("bearer ", "");
            JwtSecurityToken tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            Claim claimData = tokenS.Claims.FirstOrDefault(cl => cl.Type.ToUpper() == claim.ToUpper());

            if (claimData == null || string.IsNullOrEmpty(claimData.Value))
                throw new UnauthorizedAccessException();

            return claimData.Value;
        }

        public static bool IsAdmin(string token)
        {
            string rol = GetClaimValue(token, TypeClaims.Rol);
            bool result = rol == Roles.Admin;

            return result;
        }


        public static bool IsYourUser(string token, Guid userId)
        {
            string tokenUserId = GetClaimValue(token, TypeClaims.IdUser);
            bool result = tokenUserId == userId.ToString();

            return result;
        }


        public static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3)
                throw new BusinessException("El nombre del tipo de mensaje debe ser al menos 3 caracteres");

            if (!Regex.Match(name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
                throw new BusinessException("El nombre no puede tener caracteres especiales");

        }

        public static bool ValidateEmail(String email)
        {
            bool result = false;
            string expresion = "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                    result = true;
            }
            return result;
        }

        public static bool ValidImageExtencion(string extension)
        {
            bool result = false;

            string[] extensionValid =
            {
                ".jpg",".png",".jpeg",".gif",".bmp"
            };

            for (int i = 0; i < extensionValid.Length - 1; i++)
            {
                if (extension.Equals(extensionValid[i]))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return $"{Guid.NewGuid().ToString()}{Path.GetExtension(fileName)}";
        }

        public static string TextBase64Decrypt(string text)
        {
            try
            {
                text = EncryptProvider.Base64Decrypt(text);
            }
            catch (Exception)
            {
                Console.WriteLine("Ya estaba desencriptada");
            }

            return text;
        }
        public static string GetGender(int? genderId)
        {
            string gender = string.Empty;
            if (genderId != null)
            {
                switch (genderId)
                {
                    case (int)Gender.Masculino:
                        gender = Gender.Masculino.ToString();
                        break;
                    case (int)Gender.Femenino:
                        gender = Gender.Femenino.ToString();
                        break;
                    default:
                        gender = Gender.Otro.ToString();
                        break;
                }
            }

            return gender;
        }
        public static string GetRole(int roleId)
        {
            string rol = string.Empty;
            switch (roleId)
            {
                case (int)Rol.Admin:
                    rol = Roles.Admin;
                    break;
                case (int)Rol.Employee:
                    rol = Roles.Employee;
                    break;
                default:
                    rol = Roles.Default;
                    break;
            }

            return rol;
        }

        public static int GenerateCodeInt(int longitud)
        {
            string caracteres = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < longitud--)
            {
                res.Append(caracteres[rnd.Next(caracteres.Length)]);
            }

            return Convert.ToInt32(res.ToString());
        }

    }
}