using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Profiles
{
    public class ContactService : IContactService
    {
        private DataContext DBContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        #region Builder
        public ContactService(DataContext dBContext, IUnitOfWork unitOfWork, IImageService imageService)
        {
            DBContext = dBContext;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        #endregion

        #region Methods

        public Contact Get(Guid idContact, Guid idUser)
        {
            return _unitOfWork.ContactRepository.FirstOrDefault(x => x.Id == idContact
                                                                  && x.ProfileId == idUser);
        }

        public object Post(Contact contact)
        {

            contact.Id = Guid.NewGuid();
            contact.DateTime = DateTime.Now;

            if (contact.ProfileId == null || contact.ProfileId.ToString().Length < 6)
                return new { error = "La url de la contactn debe ser de al menos 5 caracteres " };
            Profile profile = DBContext.Profiles.Where(p => p.Id == contact.ProfileId).FirstOrDefault();
            if (profile == null)
                return new { error = "El id no coincide con ningun perfil " };
            Profile profileContact = DBContext.Profiles.Where(p => p.Id == contact.ContactProfileId).FirstOrDefault();
            if (profileContact == null)
                return new { error = "El id de contacto no coincide con ningun perfil " };
            Contact repeatedContact = DBContext.Contacts.Where(f => f.ProfileId == contact.ProfileId && f.ContactProfileId == contact.ContactProfileId).FirstOrDefault();
            if (repeatedContact != null)
            {
                throw new BusinessException("El perfil ya tiene agregado al contacto");
            }


            DBContext.Contacts.Add(contact);
            DBContext.SaveChanges();

            return DBContext.Contacts.Where(p => p.Id == contact.Id).First();
        }

        public object AddMultipleContactsByPhones(Guid userId, List<ContactToInvite> phoneContacts)
        {
            User user = DBContext.Users.Where(p => p.Id == userId).FirstOrDefault();
            if (user == null) return new { error = "El id no coincide con ningun usuario " };
            List<ContactToInvite> contactsToInvite = new List<ContactToInvite>();
            List<Contact> contacts = new List<Contact>();
            foreach (ContactToInvite phoneContact in phoneContacts)
            {
                var phoneConverted = (string)phoneContact.Phone.Replace(" ", "");
                phoneConverted = (string)phoneContact.Phone.Replace("-", "");

                Profile profile = DBContext.Profiles.Where(p => p.Phone.ToUpper().Contains(phoneConverted.ToUpper())).FirstOrDefault();
                if (profile != null)
                {
                    Contact repeated = DBContext.Contacts.Where(c => c.ProfileId == userId && c.ContactProfileId == profile.Id).FirstOrDefault();
                    if (repeated == null)
                    {
                        Contact c = new Contact();
                        c.Id = Guid.NewGuid();
                        c.DateTime = DateTime.Now;
                        c.ProfileId = userId;
                        c.Phone = phoneConverted;
                        c.ContactProfileId = profile.Id;
                        contacts.Add(c);
                    }
                    else
                    {
                        repeated.DateTime = DateTime.Now;
                        DBContext.Contacts.Update(repeated);
                        DBContext.SaveChanges();
                    }

                }
                else
                {

                    contactsToInvite.Add(phoneContact);
                }
            }
            DBContext.Contacts.AddRange(contacts);
            DBContext.SaveChanges();
            return contactsToInvite;

        }

        public object AddContactById(Guid userId, Guid contactId)
        {
            if (userId == contactId)
                return new { error = "No se puede agregar a si mismo " };

            Profile profileFrom = DBContext.Profiles.Where(p => p.Id == userId).FirstOrDefault();
            if (profileFrom == null)
                return new { error = "El usuario debe estar logueado" };

            Profile profileTo = DBContext.Profiles.Where(p => p.Id == contactId).FirstOrDefault();
            if (profileTo == null)
                return new { error = "El id no coincide con ningun perfil " };

            Contact repeated = DBContext.Contacts.Where(c => c.ProfileId == userId && c.ContactProfileId == contactId).FirstOrDefault();
            if (repeated == null)
            {
                Contact c = new Contact();
                c.Id = Guid.NewGuid();
                c.DateTime = DateTime.Now;
                c.ProfileId = userId;
                c.ContactProfileId = contactId;
                DBContext.Contacts.Add(c);
                DBContext.SaveChanges();
                return c;
            }
            else
            {
                throw new BusinessException("El perfil ya tiene agregado al contacto");
            }
        }

        public void AddSupportChatContact(Guid idPrfole)
        {
            User userChat = DBContext.Users.Where(p => p.UserName == "admin").FirstOrDefault();

            Contact supportContact = DBContext.Contacts.Where(p => p.ProfileId == idPrfole
                                                && p.ContactProfileId == userChat.Id).FirstOrDefault();
            if (supportContact == null)
            {
                Contact contact = new Contact();
                contact.Id = Guid.NewGuid();
                contact.DateTime = DateTime.Now;
                contact.ProfileId = idPrfole;
                contact.ContactProfileId = userChat.Id;
                DBContext.Contacts.Add(contact);
                DBContext.SaveChanges();
            }
        }

        public void AddTopmaiPayContact(Guid id)
        {
            User userChat = DBContext.Users.FirstOrDefault(p => p.UserName == "topmaipay");
            if (userChat.ProfileId == null)
            {
                return;
            }
            Contact supportContact = DBContext.Contacts
                                                .FirstOrDefault(p => p.ProfileId == id
                                                                        && p.ContactProfileId == userChat.ProfileId);
            if (supportContact == null)
            {
                Contact contact = new Contact();
                contact.Id = Guid.NewGuid();
                contact.DateTime = DateTime.Now;
                contact.ProfileId = id;
                contact.ContactProfileId = userChat.Id;
                DBContext.Contacts.Add(contact);
                DBContext.SaveChanges();
            }

        }
        public object Put(Contact newContact)
        {
            var idContact = newContact.Id;
            if (idContact == null || idContact.ToString().Length < 6) return new { error = "Ingrese un id de contactn válido " };

            Contact? contact = DBContext.Contacts.Where(c => c.Id == idContact && newContact.Id != null).FirstOrDefault();
            if (contact == null) return new { error = "El id no coincide con ninguna contactn " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newContact.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newContact) != null && propertyInfo.GetValue(newContact).ToString() != "")
                {
                    propertyInfo.SetValue(contact, propertyInfo.GetValue(newContact));

                }

            }

            DBContext.Entry(contact).State = EntityState.Modified;
            DBContext.SaveChanges();

            return contact;
        }

        public List<ContactDto> GetMyContacts(Guid idUser)
        {
            //AddSupportChatContact(idUser);
            //AddTopmaiPayContact(idUser);

            //El idUser es el mismo profileId
            var listContact = _unitOfWork.ContactRepository.FindAll(x => x.ProfileId == idUser,
                                                                   p => p.ContactProfile.Image,
                                                                   c => c.ContactProfile.Country).ToList();

            List<ContactDto> contats = listContact.Select(x => new ContactDto()
            {
                IdContact = x.Id,
                Locked=x.Locked,
                City = x.ContactProfile.City,
                ContactProfileId = x.ContactProfileId,
                DateTime = x.DateTime,
                Description = x.ContactProfile.Description,
                Name = x.ContactProfile.Name,
                LastName = x.ContactProfile.LastName,
                Latitude = x.ContactProfile.Latitude,
                Lenguages = x.ContactProfile.Lenguages,
                Longitude = x.ContactProfile.Longitude,
                Phone = x.ContactProfile.Phone,
                PostalCode = x.ContactProfile.PostalCode,
                ProfileId = x.ProfileId,
                Sales = x.ContactProfile.Sales,
                State = x.ContactProfile.State,
                WalletId = x.ContactProfile.WalletId,
                ProfileUrl = x.ContactProfile.ProfileUrl,
                StrCountry = x.ContactProfile?.Country?.Name,
                StrGender = Helper.GetGender(x.ContactProfile?.GenderId),
                UrlImage = _imageService.GetUrlImage(x.ContactProfile?.Image)
            }).ToList();

            return contats;
        }

        public async Task<bool> Delete(Guid idContact, Guid idUser)
        {
            Contact contact = Get(idContact, idUser);
            if (contact == null)
                throw new BusinessException("No existe el contacto a eliminar");

            _unitOfWork.ContactRepository.Delete(contact);

            return await _unitOfWork.Save() > 0;
        }

        public async Task<bool> BlockContact(Guid idContact, Guid idUser)
        {
            Contact contact = Get(idContact, idUser);
            if (contact == null)
                throw new BusinessException("No existe el contacto a bloquear");

            contact.Locked=true;
            _unitOfWork.ContactRepository.Update(contact);

            return await _unitOfWork.Save() > 0;
        }

        public async Task<bool> UnblockContact(Guid idContact, Guid idUser)
        {
            Contact contact = Get(idContact, idUser);
            if (contact == null)
                throw new BusinessException("No existe el contacto a bloquear");

            contact.Locked = false;
            _unitOfWork.ContactRepository.Update(contact);

            return await _unitOfWork.Save() > 0;
        }

        #endregion
    }
}
