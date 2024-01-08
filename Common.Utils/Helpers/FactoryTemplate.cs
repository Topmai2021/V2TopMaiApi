using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Common.Utils.Enums.Enums;

namespace Common.Utils.Helpers
{
    public static class FactoryTemplate
    {
        private readonly static string Namespace = "Common.Utils.";

        public static string TemplateEmail(TypeTemplateHtml type)
        {
            Assembly _assembly;
            StreamReader _textStreamReader;

            string fileName = string.Empty;
            switch (type)
            {
                case TypeTemplateHtml.ConfirmationRegisterEmail:
                    fileName = $"TemplateHtml.EmailConfirmationRegister.html";
                    break;
                case TypeTemplateHtml.RecuperarCuentaEmail:
                    fileName = $"TemplateHtml.EmailRecuperation.html";
                    break;
                default:
                    break;
            }

            _assembly = Assembly.GetExecutingAssembly();
            _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream($"{Namespace}{fileName}")!);
            string plantilla = _textStreamReader.ReadToEnd();

            return plantilla;
        }

    }
}
