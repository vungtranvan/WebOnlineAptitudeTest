using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.User
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "This field is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }
        public bool? Remember { get; set; } = false;
    }
}