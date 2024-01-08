namespace Common.Utils.Enums
{
    public class Enums
    {
        public enum State
        {
            Aceptada_Sell = 1,
            ContraparteContactada_Devolution = 2,
            Devolucion_Solicitada_Sell = 3,
            Aceptada_Devolution = 4,
            Pendiente_Payment = 5,
            Aprobado_Movement = 6,
            Devuelta_Sell = 7,
            Rechazada_IdentityValidation = 8,
            Enviada_Sell = 9,
            Rechazada_Devolution = 10,


            VendedorCalificado_Sell = 11,
            Pendiente_Devolution = 12,
            CompradorCalificado_Sell = 13,
            Confirmada_SellRequest = 14,
            Recibida_Sell = 15,
            Pendiente_SellRequest = 16,
            Aprobada_IdentityValidation = 17,
            EnDisputa_Devolution = 18,
            Aceptada_SellRequest = 19,
            Cancelado_Movement = 20,

            Acreditada_Devolution = 21,
            RevisandoInformacionContraparte_Devolution = 22,
            FechaEntregaActualizada_Sell = 23,
            CodigoRastreoCargado_Sell = 24,
            Rechazado_Payment = 25,
            Acreditado_Payment = 26,
            Pendiente_Movement = 27,
            Pendiente_IdentityValidation = 28,
            Rechazada_SellRequest = 29,
            DevolucionCancelada_Sell = 30,

            EnProceso_Transaction = 31,
            Completada_Transaction = 32,
            Cancelada_Transaction = 33,
            ReferenciaPagoGenerada_Transaction = 34,
            RecargaWalletInvalida_Transaction = 35,
        }

        public enum PaymentMethod
        {
            Normal = 1,
            Instantaneo = 2,
            RecargaWallet = 3
        }

        public enum MovementType
        {
            Input = 1,
            Output = 2
        }

        public enum Currency
        {
            USD = 1,
            MXN = 2
        }

        public enum MessageType
        {
            Emoticon = 1,
            Localizacion = 2,
            Video = 3,
            Imagen = 4,
            Normal = 5,
            Venta = 6,
            Oferta = 7,
            Audio = 8,
            Contacto = 9,
            Pago = 10,
        }

        public enum ChatType
        {
            TopmaiPay = 1,
            Soporte = 2,
            Normal = 3
        }

        public enum Rol
        {
            Employee = 1,
            Admin = 2,
            Default = 3
        }

        public enum Gender
        {
            Masculino = 1,
            Femenino = 2,
            Otro = 3,
        }
        public enum TypeCodeValidation
        {
            Unknown = 0,
            Email = 1,
            Phone = 2,
            LogitudCode = 6,
        }

        public enum TypeTemplateHtml
        {
            ConfirmationRegisterEmail = 1,
            RecuperarCuentaEmail = 2,
        }
    }
}
