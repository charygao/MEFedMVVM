using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVMDemo.Services.Contracts;
using MEFedMVVMDemo.Services.Models;
using MEFedMVVM.ViewModelLocator;

namespace MEFedMVVMDemo.Services.DesignTime
{
    [ExportService(ServiceType.DesignTime, typeof(IUsersService))]
    public class DesignTimeUsersService : IUsersService
    {
        #region IUsersService Members

        public IList<Models.User> GetAllUsers()
        {
            return new List<User>
            {
                new User { Name = "Marlon", Surname = "Grech", Age = 24 },
                new User { Name = "Peter", Surname = "O'Hanlon", Age = 35 },
                new User { Name = "Sasha", Surname = "Barber", Age = 28 },
                new User { Name = "Glenn", Surname = "Block", Age = 31 },
                new User { Name = "Josh", Surname = "Smith", Age = 60 },
            };
        }

        #endregion
    }
}
