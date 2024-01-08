using System;
using Common.Utils.Enums;

namespace TopMai.Domain.DTO.CodeValidation
{
	public class ChangeForgottenPassword
	{
        public string UserLogin { get; set; }

        public int Code { get; set; }

        public Enums.TypeCodeValidation Type { get; set; }

        public int TypeAsInt => (int)Type;
    }
}

