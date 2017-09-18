
using System;
using System.Collections.Generic;
using M4Core.Entities;
using M4Core.Enums;

namespace M4.M4v2.Authentication.Login
{
    public class Guest
    {
        private static Guest _guest;

        public static Guest Instace()
        {
            return _guest ?? new Guest();
        }

        public LoginAuthentication DataGuest()
        {
            LoginAuthentication guest = new LoginAuthentication
                                            {
                                                Login = "Guest",
                                                Password = "123456",
                                                LastDate = DateTime.Now,
                                                LimitDate = DateTime.Now.AddDays(5),
                                                Remember = false,
                                                Features = Features()
                                            };

            return guest;
        }

        public List<Features> Features()
        {
            List<Features> features = new List<Features>
                                          {
                                              new Features
                                                  {
                                                      Description = EFeatures.HISTORICDATA,
                                                      Permission = EPermission.Restringido
                                                  },
                                                  new Features
                                                  {
                                                      Description = EFeatures.NEW_CHART,
                                                      Permission = EPermission.Permitido
                                                  }
                                          };

            return features;
        }
    }
}
