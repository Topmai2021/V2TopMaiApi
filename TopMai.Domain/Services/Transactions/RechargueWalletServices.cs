using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Resources;
using Infraestructure.Core.Dapper;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Transactions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Transactions.HistoricalTransactions;
using TopMai.Domain.DTO.Transactions.RechargueWallet;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Domain.Services.Transactions.Interfaces;
using TopMai.Domain.Services.Users.Interfaces;
using static Microsoft.AspNetCore.Internal.AwaitableThreadPool;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TopMai.Domain.Services.Transactions
{
    public class RechargueWalletServices : IRechargueWalletServices
    {
        #region Attributes
        private DataContext DBContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly ITransacionServices _transacion;
        private readonly IImageService _imageService;
        private readonly IProfileService _profile;
        private readonly IHostingEnvironment _hostingEnvironment;


        #endregion

        #region Builder
        public RechargueWalletServices(DataContext dBContext, IUnitOfWork unitOfWork,
                                       IConfiguration configuration,
                                       ITransacionServices transacionServices,
                                       IImageService imageService,
                                       IProfileService profileService,
                                       IHostingEnvironment hostingEnvironment)
        {
            DBContext = dBContext;
            _unitOfWork = unitOfWork;
            _config = configuration;
            _transacion = transacionServices;
            _imageService = imageService;
            _profile = profileService;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region Methods

        public List<ConsultAllRechargueWallet_Dto> GetAllRechargueWallet()
        {

            IEnumerable<RechargueWallet> listRechargueWallet = _unitOfWork.RechargueWalletRepository.FindAll(x => (x.IdStatus == (int)Enums.State.ReferenciaPagoGenerada_Transaction
                                                                                                               || x.IdStatus == (int)Enums.State.EnProceso_Transaction),
                                                                                                             s => s.IdStatusNavigation,
                                                                                                             t => t.IdTypeOrigenRechargueNavigation);
            List<ConsultRechargueWallet_Dto> result = listRechargueWallet.Select(x => new ConsultRechargueWallet_Dto
            {
                IdRechargue=x.IdUserOrigen,
                Amount = x.Amount,
                Status = x.IdStatusNavigation.Name,
                PaymentReference = x.PaymentReference,
                UpdateDate = x.UpdateDate,
                TypeOrigenRechargue = x.IdTypeOrigenRechargueNavigation.TypeOrigen
            }).ToList();


            List<ConsultAllRechargueWallet_Dto> result2 = new List<ConsultAllRechargueWallet_Dto>();

            foreach (var x in result)
            {
                var profile = DBContext.Profiles.FirstOrDefault(u => u.Id == x.IdRechargue);
                //var profile = _profile.GetProfile(x.IdUserOrigen);
                var consultRechargueWalletDto = new ConsultAllRechargueWallet_Dto
                {
                    Amount = x.Amount,
                    Status = x.Status,
                    PaymentReference = x.PaymentReference,
                    UpdateDate = x.UpdateDate,
                    TypeOrigenRechargue = x.TypeOrigenRechargue,
                    Profiles = profile
                };

                result2.Add(consultRechargueWalletDto);

            }


            return result2;
        }

        public List<ConsultAllRechargueWallet_Dto> GetAllRechargueWalletByStatus(int Status)
        {

            IEnumerable<RechargueWallet> listRechargueWallet = _unitOfWork.RechargueWalletRepository.FindAll(x => (x.IdStatus == Status),
                                                                                                              s => s.IdStatusNavigation,
                                                                                                              t => t.IdTypeOrigenRechargueNavigation);
            List<ConsultRechargueWallet_Dto> result = listRechargueWallet.Select(x => new ConsultRechargueWallet_Dto
            {
                IdRechargue = x.IdUserOrigen,
                Amount = x.Amount,
                Status = x.IdStatusNavigation.Name,
                PaymentReference = x.PaymentReference,
                UpdateDate = x.UpdateDate,
                TypeOrigenRechargue = x.IdTypeOrigenRechargueNavigation.TypeOrigen
            }).ToList();


            List<ConsultAllRechargueWallet_Dto> result2 = new List<ConsultAllRechargueWallet_Dto>();

            foreach (var x in result)
            {
                var profile = DBContext.Profiles.FirstOrDefault(u => u.Id == x.IdRechargue);
                //var profile = _profile.GetProfile(x.IdUserOrigen);
                var consultRechargueWalletDto = new ConsultAllRechargueWallet_Dto
                {
                    Amount = x.Amount,
                    Status = x.Status,
                    PaymentReference = x.PaymentReference,
                    UpdateDate = x.UpdateDate,
                    TypeOrigenRechargue = x.TypeOrigenRechargue,
                    Profiles = profile
                };

                result2.Add(consultRechargueWalletDto);

            }


            return result2;
        }

        public async Task<List<ConsultRechargueWallet_Dto>> GetAllRechargueByWallet(Guid idWallet, Guid idUser)
        {
            //El IdUser es el mismo IdProfile
            var profile = _profile.GetProfile(idUser);
            if (profile.WalletId != idWallet)
                throw new BusinessException("Solo el propietario de la Wallet puede realizar la operación.");


            string connection = _config.GetConnectionString("DefaultConnection");
            DapperHelper.Instancia.ConnectionString = connection;
            var filter = new
            {
                idWallet = idWallet,
            };
            var result = await DapperHelper.Instancia.ExecuteStoreProcedure<ConsultRechargueWallet_Dto>(GeneralMessages.SP_ConsultarRecargarPorWallet, filter);

            List<ConsultRechargueWallet_Dto> list = result.Select(x => new ConsultRechargueWallet_Dto()
            {
                Amount = x.Amount,
                IdRechargue = x.IdRechargue,
                Observation = x.Observation,
                PaymentReference = x.PaymentReference,
                Status = x.Status,
                TypeOrigenRechargue = x.TypeOrigenRechargue,
                UpdateDate = x.UpdateDate,
                UrlImage = UrlImage(x.UrlImage)
            }).ToList();

            return list;
        }


        private string UrlImage(string url)
        {
            string img = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                string serverPath = _config.GetSection("GeneralSettings").GetSection("ServerPath").Value;
                string pathImagen = _config.GetSection("GeneralSettings").GetSection("PathImagen").Value;
                img = $"{serverPath}{pathImagen}{url}";
            }
            else
            {
                string serverPath = _config.GetSection("GeneralSettings").GetSection("ServerPath").Value;
                string notAvailable = _config.GetSection("GeneralSettings").GetSection("PathImagNotAvailable").Value;
                img = $"{serverPath}{notAvailable}";
            }

            return img;
        }

        public async Task<ResultReference_Dto> GetPaymentReference(ReferencePayment_Dto reference, Guid idUser)
        {
            if (reference.Amount <= 0)
                throw new BusinessException("El monto a recargar debe ser mayor a $0");

            //El IdUser es el mismo IdProfile
            var profile = _profile.GetProfile(idUser);
            if (profile.WalletId != reference.IdWallet)
                throw new BusinessException("Solo el propietario de la Wallet puede realizar la operación.");

            RechargueWallet rechargue = new RechargueWallet()
            {
                Id = Guid.NewGuid(),
                Amount = reference.Amount,
                IdWallet = reference.IdWallet,
                IdTypeOrigenRechargue = reference.IdTypeOrigenRechargue,
                RegistrationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IdStatus = (int)Enums.State.ReferenciaPagoGenerada_Transaction,
                PaymentReference = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Observation = "Se genera Referencia de pago",
                IdUserOrigen = idUser
            };

            var rwExist = _unitOfWork.RechargueWalletRepository.FirstOrDefault(x => x.IdWallet == reference.IdWallet
                                                                                                && (x.IdStatus == (int)Enums.State.ReferenciaPagoGenerada_Transaction
                                                                                                 || x.IdStatus == (int)Enums.State.EnProceso_Transaction));
            if (rwExist != null)
            {
                StringBuilder message = new StringBuilder();
                message.Append($"Estimado usuario, actualmente ya tiene una orden de pago con la referencia: [{rwExist.PaymentReference}], por un valor de:[{rwExist.Amount}]. {Environment.NewLine}");
                message.Append("Por favor cancele la orden existente, o realice el pago de la misma para poder generar una nueva.");
                throw new BusinessException(message.ToString());
            }

            _unitOfWork.RechargueWalletRepository.Insert(rechargue);
            await _unitOfWork.Save();

            return new ResultReference_Dto()
            {
                Amount = rechargue.Amount,
                PaymentReference = rechargue.PaymentReference,
                RegistrationDate = rechargue.RegistrationDate
            };
        }

        public async Task<bool> ConfirmPaymentReference(ConfirmPaymentReference_Dto confirm, Guid idUser)
        {
            if (string.IsNullOrEmpty(confirm.PaymentReference))
                throw new BusinessException("La referencia de pago es obligatoria, por favor seleccionarla.");

            RechargueWallet reference = _unitOfWork.RechargueWalletRepository.FirstOrDefault(x => x.PaymentReference == confirm.PaymentReference);
            if (reference == null)
                throw new BusinessException("La referencia de pago no se encuentra disponible, por favor verifiquela.");

            if (reference.IdUserOrigen != idUser)
                throw new BusinessException("Solo el usuario que generó la referencia de pago, puede confirmarla.");

            if (reference.IdStatus == (int)Enums.State.EnProceso_Transaction)
                throw new BusinessException("El pago ya ha sido confirmado, nuestro equipo lo está evaluando.");

            if (reference.IdStatus != (int)Enums.State.ReferenciaPagoGenerada_Transaction)
                throw new BusinessException("Esta referencia de pago no se encuentra disponible para confirmar. ");

            using (var db = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var evidencia = await _imageService.UploadImageFile(idUser.ToString(), confirm.Evidencia, _hostingEnvironment);

                    reference.UpdateDate = DateTime.Now;
                    reference.IdStatus = (int)Enums.State.EnProceso_Transaction;
                    reference.ImagenId = evidencia.Id;
                    reference.Observation = "Se está confirmando el pago por nuestro equipo Topmai.";

                    bool result = await UpdateRechargueWallet(reference);
                    if (result)
                        await db.CommitAsync();
                    else
                        await db.RollbackAsync();

                    return result;
                }
                catch (BusinessException business)
                {
                    await db.RollbackAsync();
                    throw new BusinessException(business.Message);
                }
                catch (Exception ex)
                {
                    await db.RollbackAsync();

                    throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                }
            }
        }

        public async Task<bool> PaymentApproved(Guid idRechargue, Guid idUser)
        {
            RechargueWallet rechargue = _unitOfWork.RechargueWalletRepository.FirstOrDefault(x => x.Id == idRechargue);
            if (rechargue == null)
                throw new BusinessException("No encontró la entidad, por favor vuelva a consultar");

            if (rechargue.IdStatus != (int)Enums.State.EnProceso_Transaction)
                throw new BusinessException("Esta Recarga de Wallet no se encuentra disponible para Aprobar.");

            rechargue.IdStatus = (int)Enums.State.Completada_Transaction;
            rechargue.UpdateDate = DateTime.Now;
            rechargue.Observation = "Pago confirmado, proceso completado.";
            rechargue.IdUserApprover = idUser;
            using (var db = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    string profileName = _config.GetSection("Profiles").GetSection("Admin").GetSection("Name").Value;
                    string profileLastName = _config.GetSection("Profiles").GetSection("Admin").GetSection("LastName").Value;
                    Profile profileAdmin = _profile.GetProfile(profileLastName, profileName);

                    string amountTopMai = _config.GetSection("GeneralSettings").GetSection("AmountRechargueTopmai").Value;
                    await _transacion.TransactionRechargueWallet(new RechargueTopMai_Dto()
                    {
                        Amount = rechargue.Amount,
                        AmountTopMai = Convert.ToInt32(amountTopMai),
                        IdWalletOrigen = (Guid)profileAdmin.WalletId,
                        IdWalletDestination = rechargue.IdWallet,
                        IdPaymentMethods = (int)Enums.PaymentMethod.RecargaWallet,
                        IdUser = idUser
                    });

                    await UpdateRechargueWallet(rechargue);

                    await db.CommitAsync();
                }
                catch (Exception ex)
                {
                    await db.RollbackAsync();

                    throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                }
            }


            return true;
        }

        public async Task<bool> CancelRechargueWalletByUser(Guid idRechargue, Guid idUser)
        {
            RechargueWallet rechargue = _unitOfWork.RechargueWalletRepository.FirstOrDefault(x => x.Id == idRechargue);
            if (rechargue == null)
                throw new BusinessException("No encontró la entidad, por favor vuelva a consultar");

            if (rechargue.IdUserOrigen != idUser)
                throw new BusinessException("Solo el usuario que creó La solicitud de Recarga Wallet, puede cancelarla.");

            if (rechargue.IdStatus != (int)Enums.State.ReferenciaPagoGenerada_Transaction)
                throw new BusinessException("Solo puede cancelar las recargas que se encuentren en estado [Referencia de Pago Generada].");

            rechargue.IdStatus = (int)Enums.State.Cancelada_Transaction;

            return await UpdateRechargueWallet(rechargue);

        }

        private async Task<bool> UpdateRechargueWallet(RechargueWallet rechargue)
        {
            _unitOfWork.RechargueWalletRepository.Update(rechargue);

            return await _unitOfWork.Save() > 0;
        }

        #endregion

    }
}
