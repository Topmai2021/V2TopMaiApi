using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Locations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Locations.Interfaces;
using TopMai.Domain.Services.Chats.Interfaces;
namespace TopMai.Domain.Services.Locations
{
    public class ShippmentService : IShippmentService
    {
        private DataContext DBContext;
        private IMessageService _messageService;
        #region Builder
        public ShippmentService(DataContext dBContext,IMessageService messageService)
        {
            this.DBContext = dBContext;
            _messageService = messageService;
        }
        #endregion

        #region Methods
        public List<Shippment> GetAll()
        {
            List<Shippment> shippmentes = DBContext.Shippments.OrderByDescending(x => x.Id).ToList();

            return shippmentes;


        }

        public Shippment Get(Guid id)
        {
            return DBContext.Shippments.FirstOrDefault(a => a.Id == id);

        }

        public object Post(Shippment shippment)
        {

            shippment.Id = Guid.NewGuid();
            shippment.Deleted = false;
            shippment.CreationDateTime = DateTime.Now;
            shippment.Status = 2;

            Address addressFrom = DBContext.Addresses.FirstOrDefault(a => a.Id == shippment.AddressFromId);
            if (addressFrom == null) return new { error = "La dirección de origen no existe" };
            Address addressTo = DBContext.Addresses.FirstOrDefault(a => a.Id == shippment.AddressToId);
            if (addressTo == null) return new { error = "La dirección de destino no existe" };



            DBContext.Shippments.Add(shippment);
            DBContext.SaveChanges();


            _messageService.CreateSupportMessage((Guid)shippment.ProfileId
            ,"<h2>Solicitud de envío recibida</h2>"+
            "<p>Fecha de creación: "+shippment.CreationDateTime + " </p>" +
            "<br>"+
            "<h3>DIRECCIÓN ORIGEN : </h3>"+
            "<p><b>Ciudad:</b>"+addressFrom.City + " </p>" +
            "<p><b>Estado:</b>"+addressFrom.Province + " </p> " +
            "<p><b>Calle:</b>"+addressFrom.Street + " </p> " +
            "<p><b>Código postal: </b>"+addressFrom.PostalCode + "  </p>" +
            "<p><b>Nombre: </b> "+addressFrom.Name + "  </p>" +
            "<p><b>Apellido: </b> "+addressFrom.LastName + "  </p>" +
            "<p><b>E-Mail: </b>"+addressFrom.Mail + "  </p>" +
            "<p><b>Celular: </b>"+addressFrom.Phone + "  </p>" +
            "<br>"+
            "<h3>DIRECCIÓN DESTINO :</h3> "+
            "<p><b>Ciudad: </b>"+addressTo.City + "  </p>" +
            "<p><b>Estado: </b>"+addressTo.Province + "  </p>" +
            "<p><b>Calle: </b>"+addressTo.Street + "  </p>" +
            "<p><b>Código postal: </b>"+addressTo.PostalCode + "  </p>" +
            "<p><b>Nombre: </b>"+addressTo.Name + " </p> " +
            "<p><b>Apellido: </b>"+addressTo.LastName + " </p> " +
            "<p><b>E-Mail: </b>"+addressTo.Mail + "  </p>" +
            "<p><b>Celular: </b>"+addressTo.Phone + "  </p>" +
            "<br>"+
            "<h3>DATOS DEL PAQUETE</h3>"+
            "<p><b>Proveedor: </b>"+shippment.ProviderName + "</p> " +
            "<p><b>Ancho: </b>"+shippment.Width + " cm </p>" +
            "<p><b>Alto: </b>"+shippment.Height + " cm</p> " +
            "<p><b>Largo: </b>"+shippment.Length + " cm</p>" +
            "<p><b>Cantidad: </b>"+shippment.Amount + "</p> "+
            "<p><b>Datos extra: </b>"+shippment.Personalized + " </p>" 

            );

            return DBContext.Shippments.Where(s => s.Id == shippment.Id).First();


        }

        public object Put(Shippment newShippment)
        {
            var idShippment = newShippment.Id;
            if (idShippment == null || idShippment.ToString().Length < 6) return new { error = "Ingrese un id de direccion válido " };

            Shippment? shippment = DBContext.Shippments.Where(a => a.Id == idShippment && newShippment.Id != null).FirstOrDefault();
            if (shippment == null) return new { error = "El id no coincide con ningun envío " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newShippment.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newShippment) != null && propertyInfo.GetValue(newShippment).ToString() != "")
                {
                    propertyInfo.SetValue(shippment, propertyInfo.GetValue(newShippment));

                }

            }

            DBContext.Entry(shippment).State = EntityState.Modified;
            DBContext.SaveChanges();
            return shippment;


        }

        public object Delete(Guid? id)
        {

            Shippment shippment = DBContext.Shippments.FirstOrDefault(u => u.Id == id);
            if (shippment == null) return new { error = "El id ingresado no coincide con ninguna direccion" };
            shippment.Deleted = true;
            DBContext.Entry(shippment).State = EntityState.Modified;
            DBContext.SaveChanges();
            return shippment;
        }
        #endregion
    }
}
