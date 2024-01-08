using Common.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.DTO.CodeValidation
{
    public class ValidationCode
    {
        public Guid UserId { get; set; }

        public int Code { get; set; }

        public Enums.TypeCodeValidation Type { get; set; }

        public int TypeAsInt => (int) Type;
    }
}
