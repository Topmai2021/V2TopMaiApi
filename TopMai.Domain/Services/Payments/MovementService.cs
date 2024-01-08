using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class MovementService : IMovementService
    {
        #region Attributes
        private DataContext _dBContext;
        private IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public MovementService(DataContext dBContext, IMessageService messageService, IUnitOfWork unitOfWork)
        {
            _dBContext = dBContext;
            _messageService = messageService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Services
        public async Task<List<Movement>> GetAll(int pageNumber, int pageSize)
        {
            int totalRecords = _dBContext.Movements.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            // Ensure pageNumber is within valid range
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            else if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }
            // Calculate the number of records to skip and take
            int skipAmount = (pageNumber - 1) * pageSize;
            List<Movement> movements = _dBContext.Movements.Skip(skipAmount).Take(pageSize).OrderByDescending(x => x.DateTime).ToList();
            foreach (var movement in movements)
            {
                movement.TotalCount = totalRecords;
                movement.TotalPage = totalPages;
                movement.PageNumber = pageNumber;
                movement.Status = await _dBContext.Statuses.Where(x => x.Id == movement.StatusId).FirstOrDefaultAsync();
                movement.MovementType = _dBContext.MovementTypes.Where(x => x.Id == movement.MovementTypeId).FirstOrDefault();
                movement.AuthorizedBy = _dBContext.Profiles.Where(x => x.Id == movement.AuthorizedById).FirstOrDefault();
                movement.Profile = _dBContext.Profiles.Where(x => x.Id == movement.ProfileId).FirstOrDefault();
                movement.Profile.Wallet = _dBContext.Wallets.FirstOrDefault(x => x.Id == movement.Profile.WalletId);
            }
            /**
             * .Include("Status").Include("MovementType")
            .Include("AuthorizedBy")
            .Include("Profile")
            .Include("Profile.Wallet")
            foreach (Movement movement in movements)
            {
                Status status = _dBContext.Statuses.FirstOrDefault(x => x.Id == movement.StatusId);
                MovementType movementType = _dBContext.MovementTypes.FirstOrDefault(x => x.Id == movement.MovementTypeId);

                User user = _dBContext.Users.FirstOrDefault(u => u.Id == movement.UserId);
                Wallet wallet = _dBContext.Wallets.FirstOrDefault(w => w.Id == user.WalletId);
                movement.User = user;
            }
            **/
            return movements;


        }

        public Movement Get(Guid id)
        {
            Movement movement = _unitOfWork.MovementRepository.FirstOrDefault(x => x.Id == id,
                                                                                          s => s.Status,
                                                                                          p => p.Profile,
                                                                                          m => m.MovementType);

            return movement;
        }


        public Movement Post(Movement movement)
        {
            //if (movement.Id == null) 
                movement.Id = Guid.NewGuid();

            if (movement.CurrencyId == 0)
                movement.CurrencyId = (int)Enums.Currency.MXN;

            movement.Deleted = false;
            movement.DateTime = DateTime.Now;
            if (movement.StatusId == 0)
                movement.StatusId = (int)Enums.State.Pendiente_Movement;

            movement.Profile = _dBContext.Profiles.FirstOrDefault(u => u.Id == movement.ProfileId);
            Wallet wallet = _dBContext.Wallets.FirstOrDefault(w => w.Id == movement.Profile.WalletId);
            if (wallet == null)
                throw new BusinessException("La wallet no es valida");


            if (movement.Amount == null || movement.Amount <= 0)
                throw new BusinessException("El monto no es valido");

            if (movement.MovementTypeId == (int)Enums.MovementType.Input)
            {
                if (movement.ConektaOrderId != null)
                {
                    Movement lastMovement = _unitOfWork.MovementRepository.FirstOrDefault(m => m.ProfileId == movement.ProfileId
                                                                                            && m.MovementTypeId == (int)Enums.MovementType.Input
                                                                                            && m.StatusId == (int)Enums.State.Pendiente_Movement);
                    if (lastMovement != null)
                        throw new BusinessException("Ya existe una solicitud de ingreso pendiente");

                }

            }
            else
            {

                Movement lastMovement = _unitOfWork.MovementRepository.FirstOrDefault(m => m.ProfileId == movement.ProfileId
                                                                                        && m.MovementTypeId == (int)Enums.MovementType.Output
                                                                                        && m.StatusId == (int)Enums.State.Pendiente_Movement);
                if (movement.ConektaOrderId == null)
                {


                    if (lastMovement != null)
                        throw new BusinessException("Ya existe una solicitud de retiro pendiente");

                    if (wallet.Money < movement.Amount)
                        throw new BusinessException("No hay suficiente dinero en la wallet");


                    if ((GetTotalMonthOutputByWalletId((Guid)wallet.Id) + movement.Amount) > 200000)
                    {
                        var retiro = GetTotalMonthOutputByWalletId((Guid)wallet.Id);
                        throw new BusinessException($"Con este monto el usuario supera el limite de retiro mensual de 200.000 pesos (Ya ha retirado ${retiro}) ");
                    }


                    if (movement.Detail == null || movement.Detail.Length < 1)
                        throw new BusinessException("Debe ingresar datos bancarios del usuario para realizar el retiro");

                }

                wallet.Money -= movement.Amount;
            }

            _dBContext.Wallets.Update(wallet);

            _dBContext.Movements.Add(movement);
            _dBContext.SaveChanges();
            return movement;
        }


        public async Task<Movement> CancelMovement(Guid movementId)
        {
            Movement movement = Get(movementId);
            if (movement == null)
                throw new BusinessException("El movimiento no existe");

            if (movement.StatusId == (int)Enums.State.Pendiente_Movement)
                throw new BusinessException("El movimiento ya ha sido procesado");

            if (movement.MovementTypeId == (int)Enums.MovementType.Output)
            {
                Wallet wallet = _dBContext.Wallets.FirstOrDefault(w => w.Id == movement.Profile.WalletId);
                wallet.Money += movement.Amount;
                _dBContext.Wallets.Update(wallet);
            }

            movement.StatusId = (int)Enums.State.Cancelado_Movement;
            movement.ResolutionDate = DateTime.Now;
            _unitOfWork.MovementRepository.Update(movement);
            await _unitOfWork.Save();

            return movement;
        }

        public float GetTotalMonthOutputByWalletId(Guid walletId)
        {
            Wallet wallet = _dBContext.Wallets.FirstOrDefault(w => w.Id == walletId);
            Profile profile = _dBContext.Profiles.FirstOrDefault(u => u.WalletId == wallet.Id);

            List<Movement> movements = _dBContext.Movements.Where(m => m.ProfileId == profile.Id
                                                                    && m.StatusId == (int)Enums.State.Cancelado_Movement
                                                                    && m.MovementTypeId == (int)Enums.MovementType.Output
                                                                    && m.DateTime.Value.Month == DateTime.Now.Month).ToList();

            float total = 0;
            foreach (Movement movement in movements)
            {
                total += (float)movement.Amount;
            }

            return total;
        }
        public int GetAmountOfPendingMovementInputs()
        {
            return _unitOfWork.MovementRepository.FindAll(m => m.StatusId == (int)Enums.State.Pendiente_Movement
                                                             & m.MovementTypeId == (int)Enums.MovementType.Input).Count();
        }

        public int GetAmountOfPendingMovementOutputs()
        {
            return _unitOfWork.MovementRepository.FindAll(m => m.StatusId == (int)Enums.State.Pendiente_Movement
                                                             & m.MovementTypeId == (int)Enums.MovementType.Output).Count();
        }


        public async Task<Movement> Put(Movement newMovement)
        {
            newMovement.ResolutionDate = DateTime.Now;
            var idMovement = newMovement.Id;
            if (idMovement == null || idMovement.ToString().Length < 6)
                throw new BusinessException("Ingrese un id de movimiento válido");

            Movement? movement = _dBContext.Movements.Include("Status").Include("MovementType")
                                .Where(c => c.Id == idMovement && newMovement.Id != null).FirstOrDefault();
            if (movement == null)
                throw new BusinessException("El id no coincide con ningun movimiento");

            movement.Profile = _dBContext.Profiles.FirstOrDefault(u => u.Id == movement.ProfileId);
            newMovement.Status = _dBContext.Statuses.FirstOrDefault(s => s.Id == newMovement.StatusId);

            if (newMovement.StatusId != movement.StatusId)
            {

                //acredit movement request  
                if (newMovement.StatusId == (int)Enums.State.Aprobado_Movement)
                {
                    if (movement.MovementType.Name == "Input")
                    {
                        Wallet wallet = _dBContext.Wallets.FirstOrDefault(w => w.Id == movement.Profile.WalletId);
                        wallet.Money += movement.Amount;
                        _dBContext.Wallets.Update(wallet);
                        //notify user
                        string content = JsonConvert.SerializeObject(new
                        {
                            amount = movement.Amount,
                            currency = "MXN",
                            date = movement.DateTime,
                            receiptDate = newMovement.ResolutionDate,
                            status = "Acreditado",
                            type = "Recarga",
                            id = movement.Id,
                            movementType = "Movement"
                        });

                        await _messageService.CreateTopmaiPayMessage((Guid)movement.ProfileId, content, (int)Enums.MessageType.Pago);
                    }
                    else
                    {
                        //notify user
                        string content = JsonConvert.SerializeObject(new
                        {
                            amount = movement.Amount,
                            currency = "MXN",
                            date = movement.DateTime,
                            receiptDate = movement.ResolutionDate,
                            status = "Acreditado",
                            type = "Retiro",
                            id = movement.Id,
                            movementType = "Movement"
                        });
                        await _messageService.CreateTopmaiPayMessage((Guid)movement.ProfileId, content, (int)Enums.MessageType.Pago);
                    }
                }
                //cancel movement request  
                if (newMovement.StatusId == (int)Enums.State.Cancelado_Movement)
                {

                    if (movement.MovementTypeId == (int)Enums.MovementType.Output)
                    {

                        Wallet wallet = _dBContext.Wallets.FirstOrDefault(w => w.Id == movement.Profile.WalletId);
                        wallet.Money += movement.Amount;

                        _dBContext.Wallets.Update(wallet);

                        await _messageService.CreateTopmaiPayMessage((Guid)movement.ProfileId,
                                                                    "Su solicitud de retiro de $" + movement.Amount + " ha sido cancelada", (int)Enums.MessageType.Normal);
                    }
                    else
                    {
                        await _messageService.CreateTopmaiPayMessage((Guid)movement.ProfileId
                                      , "Su solicitud de acreditación de $" + movement.Amount + " ha sido cancelada", (int)Enums.MessageType.Normal);
                    }

                }


            }

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newMovement.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newMovement) != null && propertyInfo.GetValue(newMovement).ToString() != "")
                {
                    propertyInfo.SetValue(movement, propertyInfo.GetValue(newMovement));

                }

            }

            _dBContext.Entry(movement).State = EntityState.Modified;
            _dBContext.SaveChanges();
            return movement;
        }

        public List<Movement> GetAllMovementsByUserId(Guid userId)
        {
            User user = _dBContext.Users.Include("Profile").FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            List<Movement> movements = _unitOfWork.MovementRepository.FindAll(m => m.ProfileId == user.Profile.Id)
                                                                     .OrderByDescending(m => m.DateTime).ToList();

            return movements;
        }

        public Movement GetPendingInputByUserId(Guid userId)
        {
            User user = _dBContext.Users.Include("Profile").FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            return _unitOfWork.MovementRepository.FirstOrDefault(m => m.ProfileId == user.Profile.Id
                                                                   && m.StatusId == (int)Enums.State.Cancelado_Movement
                                                                   && m.MovementTypeId == (int)Enums.MovementType.Input);

        }

        public Movement GetPendingOutputByUserId(Guid userId)
        {
            User user = _dBContext.Users.Include("Profile").FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            return _unitOfWork.MovementRepository.FirstOrDefault(m => m.ProfileId == user.Profile.Id
                                                                   && m.StatusId == (int)Enums.State.Pendiente_Movement
                                                                   && m.MovementTypeId == (int)Enums.MovementType.Output);
        }

        public List<Movement> GetSolvedInputsByUserId(Guid userId)
        {
            User user = _dBContext.Users.Include("Profile").FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            return _unitOfWork.MovementRepository.FindAll(m => m.ProfileId == user.Profile.Id
                                                            && ((m.StatusId == (int)Enums.State.Aprobado_Movement)
                                                                || (m.StatusId == (int)Enums.State.Cancelado_Movement))
                                                            && m.MovementTypeId == (int)Enums.MovementType.Input)
                                                 .OrderByDescending(m => m.ResolutionDate).ToList();
        }

        public List<Movement> GetSolvedOutputsByUserId(Guid userId)
        {
            User user = _dBContext.Users.Include("Profile").FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            return _unitOfWork.MovementRepository.FindAll(m => m.ProfileId == user.Profile.Id
                                                            && ((m.StatusId == (int)Enums.State.Aprobado_Movement)
                                                                || (m.StatusId == (int)Enums.State.Cancelado_Movement))
                                                            && m.MovementTypeId == (int)Enums.MovementType.Output)
                                                 .OrderByDescending(m => m.ResolutionDate).ToList();
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id.ToString().Length < 6)
                throw new BusinessException("Ingrese un id válido");


            Movement movement = Get(id);
            if (movement == null)
                throw new BusinessException("El id ingresado no coincide con ninguna movimiento");

            movement.Deleted = true;
            _unitOfWork.MovementRepository.Update(movement);

            return await _unitOfWork.Save() > 0;
        }

        #endregion

    }
}
