using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVMDemo.Services.Contracts;
using MEFedMVVMDemo.Services.Models;
using MEFedMVVM.ViewModelLocator;

namespace MEFedMVVMDemo.Services.Runtime
{
    [ExportService(ServiceType.Runtime, typeof(IUsersService))]
    public class UsersService : IUsersService
    {
        #region IUsersService Members

        public IList<Models.User> GetAllUsers()
        {
            //THIS IS FOR DEMO ONLY. HERE YOU SHOULD CONNECT TO THE DATA ACCESS YOU HAVE DB/WEBSERVICE whatever
            return new List<User>
            {
                new User { Name = "Runtime Marlon", Surname = "Grech", Age = 24 },
                new User { Name = "Runtime Peter", Surname = "O'Hanlon", Age = 35 },
                new User { Name = "Runtime Sasha", Surname = "Barber", Age = 28 },
                new User { Name = "Runtime Glenn", Surname = "Block", Age = 31 },
                new User { Name = "Runtime Josh", Surname = "Smith", Age = 60 },
            };
        }

        #endregion
    }
}
