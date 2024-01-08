using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Resources;
using Dapper;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Transactions;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Policy;
using TopMai.Domain.DTO.Transactions;
using TopMai.Domain.DTO.Transactions.HistoricalTransactions;
using TopMai.Domain.DTO.Transactions.RechargueWallet;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Domain.Services.Transactions.Interfaces;

namespace TopMai.Domain.Services.Transactions
{
    public class TransacionServices : ITransacionServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly IHistoricalTransactionServices _historicalTransactionServices;
        private readonly IProfileService _profileService;
        private readonly ITransactionsAccreditationServices _transactionsAccreditationServices;
        private readonly IImageService _imageService;
        #endregion

        #region Builder
        public TransacionServices(IUnitOfWork unitOfWork,
                                  IWalletService walletService,
                                  IHistoricalTransactionServices historicalTransactionServices,
                                  IProfileService profileService,
                                  ITransactionsAccreditationServices transactionsAccreditationServices,
                                  IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _historicalTransactionServices = historicalTransactionServices;
            _profileService = profileService;
            _transactionsAccreditationServices = transactionsAccreditationServices;
            _imageService = imageService;
        }
        #endregion

        #region Methods

        private Transaction GetTransaction(Guid idTransaction) => _unitOfWork.TransactiontRepository.FirstOrDefault(x => x.Id == idTransaction);
        private async Task<bool> InsertTransaction(Transaction transaction)
        {
            _unitOfWork.TransactiontRepository.Insert(transaction);

            return await _unitOfWork.Save() > 0;
        }

        public async Task NewTransactions(AddTransaction_Dto add, Guid idUser)
        {
            if (add.Amount <= 0)
                throw new BusinessException("El monto a transferir debe ser mayor a $0");

            //El IdUser es el mismo IdProfile
            var profile = _profileService.GetProfile(idUser);
            if (profile.WalletId != add.IdWalletOrigen)
                throw new BusinessException("Solo el propietario de la Wallet puede realizar la operación.");

            var walletOrigen = _walletService.Get(add.IdWalletOrigen);
            if (walletOrigen == null)
                throw new BusinessException("No se encontró la billetera del usuario que realiza la transación");

            if (walletOrigen.Money == null || add.Amount > Convert.ToDecimal(walletOrigen.Money))
                throw new BusinessException("Estimado usuario, no tiene la cantidad de dinero suficiente para realizar la operación");

            var walletDestination = _walletService.Get(add.IdWalletDestination);
            if (walletDestination == null)
                throw new BusinessException("No se encontró la billetera del usuario que recibe la transación");


            Transaction transaction = new Transaction()
            {
                Id = Guid.NewGuid(),
                IdWalletOrigen = add.IdWalletOrigen,
                IdWalletDestination = add.IdWalletDestination,
                IdPaymentMethods = add.IdPaymentMethods,
                Amount = add.Amount,
                IdStatus = (int)Enums.State.Pendiente_Movement,
                TransationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Observation = "Pendiente de liberación"
            };

            walletOrigen.Money = (walletOrigen.Money - (float)transaction.Amount);
            if (transaction.IdPaymentMethods == (int)Enums.PaymentMethod.Instantaneo)
            {
                transaction.IdStatus = (int)Enums.State.Aprobado_Movement;
                transaction.Observation = "Dinero liberado";
                walletDestination.Money = (walletDestination.Money + (float)transaction.Amount);

                using (var db = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        await _walletService.UpdateWallet(walletOrigen);
                        await _walletService.UpdateWallet(walletDestination);
                        await InsertTransaction(transaction);

                        await _historicalTransactionServices.InsertHistoricalTransaction(new AddHistoricalTransaction_Dto()
                        {
                            Observation = transaction.Observation,
                            IdStatusTransaction = transaction.IdStatus,
                            IdTransaction = transaction.Id,
                            Amount = transaction.Amount,
                        });

                        await db.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await db.RollbackAsync();

                        throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                    }
                }
            }
            else
            {

                using (var db = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var paymentMethod = _unitOfWork.PaymentMethodRepository.FirstOrDefault(x => x.Id == transaction.IdPaymentMethods);
                        await _walletService.UpdateWallet(walletOrigen);

                        transaction.Observation = string.Format(GeneralMessages.TransactionPending, DateTime.Now.AddDays(paymentMethod.AccreditationDays).ToString("dd-MMMM-yyyy"));

                        await InsertTransaction(transaction);
                        await _transactionsAccreditationServices.NewTransactionAcreditation(new AddTransactionsAccreditation_Dto()
                        {
                            IdTransaction = transaction.Id,
                            Days = paymentMethod.AccreditationDays
                        });

                        await _historicalTransactionServices.InsertHistoricalTransaction(new AddHistoricalTransaction_Dto()
                        {
                            Observation = transaction.Observation,
                            IdStatusTransaction = transaction.IdStatus,
                            IdTransaction = transaction.Id,
                            Amount = transaction.Amount,
                        });

                        await db.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await db.RollbackAsync();
                        throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                    }
                }
            }
        }

        public List<TransactionDto> GetAllTransactionsByWallet(Guid idWallet, Guid idUser)
        {
            //El IdUser es el mismo IdProfile
            var profile = _profileService.GetProfile(idUser);
            if (profile.WalletId != idWallet)
                throw new BusinessException("Solo el propietario de la Wallet puede realizar la operación.");

            var listTransactions = _unitOfWork.TransactiontRepository.FindAllSelect(x => (x.IdWalletOrigen == idWallet || x.IdWalletDestination == idWallet),
                                                                                    p => p.IdWalletDestinationNavigation.Profiles,
                                                                                    p => p.IdWalletDestinationNavigation.Profiles.Select(i => i.Image),
                                                                                    p => p.IdWalletOrigenNavigation.Profiles,
                                                                                    p => p.IdWalletOrigenNavigation.Profiles.Select(i => i.Image),
                                                                                    s => s.IdStatusNavigation,
                                                                                    mp => mp.IdPaymentMethodsNavigation,
                                                                                    h => h.HistoricalTransactions,
                                                                                    h => h.HistoricalTransactions.Select(x => x.IdStatusTransactionNavigation));

            List<TransactionDto> transactions = listTransactions.Where(w => w.IdWalletDestination == idWallet).Select(x => new TransactionDto()
            {

                Amount = x.Amount,
                IdTransation = x.Id,
                IdTypeTransaction = (int)Enums.MovementType.Input,
                TransationDate = x.TransationDate,
                Status = x.IdStatusNavigation.Name,
                StrPaymentMethods = x.IdPaymentMethodsNavigation.Name,
                UserNameProfileDestionation = x.IdWalletDestinationNavigation.Profiles.First().FullName,
                UrlImagenProfileDestionation = _imageService.GetUrlImage(x.IdWalletDestinationNavigation.Profiles.First().Image),
                UserNameProfileOrigen = x.IdWalletOrigenNavigation.Profiles.First().FullName,
                UrlImagenProfileOrigen = _imageService.GetUrlImage(x.IdWalletOrigenNavigation.Profiles.First().Image),
                HistoricalTransactions = x.HistoricalTransactions.Select(h => new ConsultHistoricalTransactions_Dto()
                {
                    Status = h.IdStatusTransactionNavigation.Name,
                    RegistrationDate = h.RegistrationDate,
                    Observation = h.Observation
                }).ToList()
            }).ToList();


            List<TransactionDto> outTransactions = listTransactions.Where(x => x.IdWalletOrigen == idWallet).Select(x => new TransactionDto()
            {
                Amount = (x.Amount * (-1)),
                IdTransation = x.Id,
                IdTypeTransaction = (int)Enums.MovementType.Output,
                TransationDate = x.TransationDate,
                Status = x.IdStatusNavigation.Name,
                StrPaymentMethods = x.IdPaymentMethodsNavigation.Name,
                UserNameProfileDestionation = x.IdWalletDestinationNavigation.Profiles.First().FullName,
                UrlImagenProfileDestionation = _imageService.GetUrlImage(x.IdWalletDestinationNavigation.Profiles.First().Image),
                UserNameProfileOrigen = x.IdWalletOrigenNavigation.Profiles.First().FullName,
                UrlImagenProfileOrigen = _imageService.GetUrlImage(x.IdWalletOrigenNavigation.Profiles.First().Image),
                HistoricalTransactions = x.HistoricalTransactions.Select(h => new ConsultHistoricalTransactions_Dto()
                {
                    Status = h.IdStatusTransactionNavigation.Name,
                    RegistrationDate = h.RegistrationDate,
                    Observation = h.Observation
                }).ToList()
            }).ToList();
            transactions.AddRange(outTransactions);

            return transactions.OrderByDescending(x => x.TransationDate).ToList();
        }

        public async Task<bool> CancelTransaction(Guid idTransaction, Guid idUser)
        {
            Transaction transaction = GetTransaction(idTransaction);
            if (transaction == null)
                throw new BusinessException("No se encontró la transacción, por favor vuelva a intentalo.");

            //El IdUser es el mismo IdProfile
            var profile = _profileService.GetProfile(idUser);
            if (profile.WalletId != transaction.IdWalletOrigen)
                throw new BusinessException("Solo el propietario de la Wallet puede realizar la operación.");


            if (transaction.IdStatus != (int)Enums.State.Pendiente_Movement)
                throw new BusinessException("Solo puede cancelar las transacciones que se encuentren en estado [Pendiente].");

            var walletOrigen = _walletService.Get(transaction.IdWalletOrigen);
            using (var db = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    walletOrigen.Money = (walletOrigen.Money + (float)transaction.Amount);
                    await _walletService.UpdateWallet(walletOrigen);

                    transaction.IdStatus = (int)Enums.State.Cancelada_Transaction;
                    transaction.Observation = "Transacción cancelada por el usuario";
                    transaction.UpdateDate = DateTime.Now;
                    await _historicalTransactionServices.InsertHistoricalTransaction(new AddHistoricalTransaction_Dto()
                    {
                        Observation = transaction.Observation,
                        IdStatusTransaction = transaction.IdStatus,
                        IdTransaction = transaction.Id,
                        Amount = transaction.Amount,
                    });
                    bool result = await UpdateTransaction(transaction);

                    await db.CommitAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    await db.RollbackAsync();

                    throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                }
            }



        }

        private async Task<bool> UpdateTransaction(Transaction transaction)
        {
            _unitOfWork.TransactiontRepository.Update(transaction);

            return await _unitOfWork.Save() > 0;
        }


        #region RechargueWallet
        public async Task TransactionRechargueWallet(RechargueTopMai_Dto add)
        {
            Wallet walletOrigen = _walletService.Get(add.IdWalletOrigen);
            if (walletOrigen == null)
                throw new BusinessException("No se encontró la billetera de TopMai, por favor reportarlo a soporte.");

            if (walletOrigen.Money == null || add.Amount > Convert.ToDecimal(walletOrigen.Money))
                await RechargueTopMai(walletOrigen, add);

            var walletDestionation = _walletService.Get(add.IdWalletDestination);
            if (walletDestionation == null)
                throw new BusinessException("No se encontró la billetera del usuario que recibe la transación");

            Transaction transaction = new Transaction()
            {
                Id = Guid.NewGuid(),
                IdWalletOrigen = add.IdWalletOrigen,
                IdWalletDestination = add.IdWalletDestination,
                IdPaymentMethods = add.IdPaymentMethods,
                Amount = add.Amount,
                IdStatus = (int)Enums.State.Aprobado_Movement,
                TransationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Observation = "Dinero liberado",
            };

            walletOrigen.Money = (walletOrigen.Money - (float)transaction.Amount);
            walletDestionation.Money = (walletDestionation.Money + (float)transaction.Amount);

            await _walletService.UpdateWallet(walletOrigen);
            await _walletService.UpdateWallet(walletDestionation);
            await InsertTransaction(transaction);

            await _historicalTransactionServices.InsertHistoricalTransaction(new AddHistoricalTransaction_Dto()
            {
                Observation = transaction.Observation,
                IdStatusTransaction = transaction.IdStatus,
                IdTransaction = transaction.Id,
                Amount = transaction.Amount,
            });
        }

        private async Task<bool> RechargueTopMai(Wallet walletOrigen, RechargueTopMai_Dto transaction)
        {
            while (transaction.AmountTopMai <= transaction.Amount)
            {
                //Incremento 
                transaction.AmountTopMai = transaction.AmountTopMai * 2;
            }

            walletOrigen.Money = walletOrigen.Money + transaction.AmountTopMai;

            RechargueTopMai rechargue = new RechargueTopMai()
            {
                Id = Guid.NewGuid(),
                Amount = transaction.AmountTopMai,
                CreationDate = DateTime.Now,
                IdWallet = walletOrigen.Id,
                IdUser = transaction.IdUser,
            };
            _unitOfWork.RechargueTopMaiRepository.Insert(rechargue);
            return await _unitOfWork.Save() > 0;
        }

        public TransactionDto GetByIdTransactionsByWallet(Guid idTransaction)
        {
            Transaction? transaction = _unitOfWork.TransactiontRepository.FirstOrDefault(x => x.Id == idTransaction);

            if (transaction == null)
                throw new BusinessException("No se encontró la transaccion con el id solicitado");

            Status? status = _unitOfWork.StatusRepository.FirstOrDefault(x => x.Id == transaction.IdStatus);

            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.WalletId == transaction.IdWalletOrigen);
            
            Profile profileDest =_unitOfWork.ProfileRepository.FirstOrDefault(x => x.WalletId == transaction.IdWalletDestination);

            return new TransactionDto()
            {
                IdTransation = transaction.Id,
                TransationDate = transaction.TransationDate,
                Status = status.Name,
                Amount = transaction.Amount,
                UserNameProfileOrigen = profile.FullName,
                UserNameProfileDestionation = profileDest.FullName,
                IdTypeTransaction = transaction.IdPaymentMethods,
                StrPaymentMethods = transaction.IdPaymentMethods.ToString(),
                HistoricalTransactions = new List<ConsultHistoricalTransactions_Dto>
                {
                    new ConsultHistoricalTransactions_Dto
                    {
                        RegistrationDate = transaction.TransationDate,
                        Status = status.Name,
                        Observation = transaction.Observation 
                    }
                }
                //UserNameProfileDestionation = wallet.Profiles.First().FullName
            };
        }
        #endregion

        #endregion

    }
}
