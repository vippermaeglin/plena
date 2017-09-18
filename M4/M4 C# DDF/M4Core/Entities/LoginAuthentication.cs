using System;
using System.Collections.Generic;
using System.Linq;
using M4Core.Enums;
using M4Utils;

namespace M4Core.Entities
{
    public class LoginAuthentication
    {
        private static LoginAuthentication _loginAuthentication;

        public static LoginAuthentication Instance()
        {
            return _loginAuthentication ?? new LoginAuthentication();
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime LastDate { get; set; }

        public DateTime LimitDate { get; set; }
        
        public bool Remember { get; set; }

        public List<Features> Features;

        public string CodeSession { get; set; }

        public DateTime BaseDate { get; set; }

        public EPermission VerifyFeature(EFeatures descriptionFeature)
        {
            if (Features.Exists(f => f.Description == descriptionFeature))
            {
                Features features = (from Features feature in Features where feature.Description.Equals(descriptionFeature) select feature).SingleOrDefault();
                return features.Permission;
            }
            else return EPermission.Permitido;
        }

        public bool Offline { get; set; }

        public UserRegister UserProfile { get; set; }

        public string IpServer {get;set;}
    }

    public class UserRegister
    {
        private static UserRegister _userRegister;
        public static UserRegister Instance()
        {
            return _userRegister ?? new UserRegister();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool VerifyPassword()
        {
            return Password == ConfirmPassword;
        }
        public bool VerifyPassword(string password,string confirmPassword)
        {
            return password == confirmPassword;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string CEP { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public EPerfil Tipo { get; set; }
    }

    public enum EPerfil
    {        
        [StringValue("MASTER")]
        Master = 1,
        [StringValue("GUEST")]
        Guest,
        [StringValue("USER_EOD")]
        User_EOD,
        [StringValue("USER_RT")]
        User_RT,
    }

}