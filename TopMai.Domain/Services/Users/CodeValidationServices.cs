using System;
using System.ComponentModel.DataAnnotations;
using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Users;
using TopMai.Domain.DTO.CodeValidation;
using TopMai.Domain.Services.Emails.Interfaces;
using TopMai.Domain.Services.Users.Interfaces;
using static Common.Utils.Enums.Enums;
using static Humanizer.On;

namespace TopMai.Domain.Services.Users
{
    public class CodeValidationServices : ICodeValidationServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        #endregion

        #region Builder
        public CodeValidationServices(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        #endregion

        #region Methods
        public async Task<bool> GenerateCodeEmail(Guid idUser)
        {
            User user = GetUser(idUser);
            if (user == null)
                throw new BusinessException("No hay un usuario especificado para enviarle el código");

            if (string.IsNullOrEmpty(user.Mail))
                throw new BusinessException("No se ha especificado un Email para enviarle el código");

            CodeValidation code = GetCodeValidation(idUser, (int)TypeCodeValidation.Email);
            if (code != null)
                await DeleteCodeValidationUser(code);

            int generatedCode = Helper.GenerateCodeInt((int)TypeCodeValidation.LogitudCode);
            bool result = await InsertCodeValidation(idUser, (int)TypeCodeValidation.Email, generatedCode);
            if (result)
            {
                //Enviar Codigo
                string template = FactoryTemplate.TemplateEmail(TypeTemplateHtml.ConfirmationRegisterEmail);
                template = template.Replace("@code", generatedCode.ToString());
                await _emailService.SendEmailAsync(new DTO.Email.MailModelDto()
                {
                    IsBodyHtml = true,
                    Subject = "Confirmación de Registro - Topmai",
                    Content = template,
                    To = new List<DTO.Email.EmailAddress>
                    {
                        new DTO.Email.EmailAddress()
                        {
                            Addres= user.Mail!,
                            DisplayName=user.UserName??string.Empty
                        },
                    }
                });
            }

            return result;
        }

        public async Task<bool> GenerateCodeEmail(string email)
        {
            var user = GetUserByEmail(email) ?? throw new BusinessException("No hay un usuario especificado para este Email");
            if ((SignupTypeEnum)user.SignupType != SignupTypeEnum.Normal)
            {
                throw new BusinessException("El usario esta registrado con " + Enum.GetName(typeof(SignupTypeEnum), user.SignupType));
            }

            CodeValidation code = GetCodeValidation(user.Id, (int)TypeCodeValidation.Email);
            if (code != null)
                await DeleteCodeValidationUser(code);

            int generatedCode = Helper.GenerateCodeInt((int)TypeCodeValidation.LogitudCode);
            bool result = await InsertCodeValidation(user.Id, (int)TypeCodeValidation.Email, generatedCode);
            if (result)
            {
                //Enviar Codigo
                string template = FactoryTemplate.TemplateEmail(TypeTemplateHtml.RecuperarCuentaEmail);
                template = template.Replace("@code", generatedCode.ToString());
                await _emailService.SendEmailAsync(new DTO.Email.MailModelDto()
                {
                    IsBodyHtml = true,
                    Subject = "Código de Recuperación - Topmai",
                    Content = template,
                    To = new List<DTO.Email.EmailAddress>
                    {
                        new DTO.Email.EmailAddress()
                        {
                            Addres= user.Mail!,
                            DisplayName=user.UserName??string.Empty
                        },
                    }
                });
            }

            return result;
        }

        public async Task<(bool, string?)> ValidateCode(ValidationCode validate)
        {
            var user = GetUser(validate.UserId) ?? throw new BusinessException("No hay un usuario especificado para este id");
            var code = GetCodeValidation(user.Id, validate.TypeAsInt) ?? throw new BusinessException("Su código ha expirado, por favor generar uno nuevo.");
            if (code.Code != validate.Code)
                throw new BusinessException("El código ingresado no coincide con el generado, por favor vuelva a intentarlo.");

            var duration = DateTime.Now - code.StartDate;
            if (duration.TotalMinutes > 30)
                throw new BusinessException("Su código ha expirado, por favor generar uno nuevo.");

            string? informationToValidate;
            if (validate.Type == TypeCodeValidation.Email)
            {
                user.VerifiedEmail = true;
                informationToValidate = user.Mail;
            }
            else
            {
                user.VerifiedPhone = true;
                informationToValidate = user.Phone;
            }

            bool result = false;
            using (var db = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    result = await UpdateUser(user);
                    if (result)
                        await DeleteCodeValidationUser(code);
                    else
                    {
                        await db.RollbackAsync();
                        throw new BusinessException("Hubo un error al realizar la operación, por favor vuelta a intentarlo");
                    }

                    await db.CommitAsync();
                }
                catch (Exception ex)
                {
                    await db.RollbackAsync();
                    throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                }
            }

            return (result, informationToValidate);
        }

        public async Task<bool> ValidateForgottenCode(ChangeForgottenPassword change)
        {
            User user;
            if (change.Type == TypeCodeValidation.Email)
            {
                user = GetUserByEmail(change.UserLogin);
            } else if (change.Type == TypeCodeValidation.Phone)
            {
                user = GetUserByPhone(change.UserLogin);
            } else
            {
                throw new BusinessException("No hay un usuario especificado para este type");
            }

            var code = GetCodeValidation(user.Id, change.TypeAsInt) ?? throw new BusinessException("Su código ha expirado, por favor generar uno nuevo.");
            if (code.Code != change.Code)
                throw new BusinessException("El código ingresado no coincide con el generado, por favor vuelva a intentarlo.");

            var duration = DateTime.Now - code.StartDate;
            if (duration.TotalMinutes > 30)
                throw new BusinessException("Su código ha expirado, por favor generar uno nuevo.");

            await DeleteCodeValidationUser(code);

            return true;
        }

        public async Task<bool> GenerateCodePhone(Guid idUser)
        {
            User user = GetUser(idUser);
            await generateCodePhone(user);

            return true;
        }

        public async Task<bool> GenerateCodePhone(string phone)
        {
            var user = GetUserByPhone(phone);
            await generateCodePhone(user);

            return true;
        }

        private async Task generateCodePhone(User user)
        {
            if (user == null)
                throw new BusinessException("No hay un usuario especificado para enviarle el código");

            if ((SignupTypeEnum)user.SignupType != SignupTypeEnum.Normal)
            {
                throw new BusinessException("El usario esta registrado con " + Enum.GetName(typeof(SignupTypeEnum), user.SignupType));
            }

            if (string.IsNullOrEmpty(user.Phone))
                throw new BusinessException("No se ha especificado un número de teléfono para enviarle el código");

            CodeValidation code = GetCodeValidation(user.Id, (int)TypeCodeValidation.Phone);
            if (code != null)
                await DeleteCodeValidationUser(code);

            int generatedCode = Helper.GenerateCodeInt((int)TypeCodeValidation.LogitudCode);
            await InsertCodeValidation(user.Id, (int)TypeCodeValidation.Phone, generatedCode);
        }

        #endregion

        #region Privates
        private async Task<bool> UpdateUser(User userUpdate)
        {
            _unitOfWork.UserRepository.Update(userUpdate);
            return await _unitOfWork.Save() > 0;
        }
        private User GetUser(Guid id) => _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == id);
        private User GetUserByEmail(string mail) => _unitOfWork.UserRepository.FirstOrDefault(u => u.Mail.ToLower() == mail.ToLower());
        private User GetUserByPhone(string phone) => _unitOfWork.UserRepository.FirstOrDefault(u => u.Phone.ToLower() == phone.ToLower());

        private async Task<bool> InsertCodeValidation(Guid idUser, int typeCode, int code)
        {
            CodeValidation codeUser = new CodeValidation()
            {
                Id = Guid.NewGuid(),
                IdUser = idUser,
                StartDate = DateTime.Now,
                TypeCode = typeCode,
                Code = code,
            };
            _unitOfWork.CodeValidationRepository.Insert(codeUser);

            return await _unitOfWork.Save() > 0;
        }

        private CodeValidation GetCodeValidation(Guid idUser, int typeCode)
        {
            CodeValidation code = _unitOfWork.CodeValidationRepository.FirstOrDefault(x => x.IdUser == idUser
                                                                                        && x.TypeCode == typeCode);

            return code;
        }

        private async Task<bool> DeleteCodeValidationUser(CodeValidation code)
        {
            _unitOfWork.CodeValidationRepository.Delete(code);

            return await _unitOfWork.Save() > 0;
        }
        #endregion
    }
}
