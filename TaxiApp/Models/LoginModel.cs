using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не введен логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не введен пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
