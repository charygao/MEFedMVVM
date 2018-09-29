using System;
using MEFedMVVM.ValidationsExtensions;

namespace MEFedMVVMDemo.Services.Models
{
    public class User : DataValidationBase
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                ValidateAndNotifyPropertyChanged(() => Name,
                    PropertyValidation.Create(() => String.IsNullOrWhiteSpace(Name), "Name cannot be left empty"));
            }
        }

        private string _surname;
        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
                ValidateAndNotifyPropertyChanged(() => Surname,
                    PropertyValidation.Create(() => String.IsNullOrWhiteSpace(Surname), "Surname cannot be left empty"));
            }
        }

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                ValidateAndNotifyPropertyChanged(() => Age,
                    PropertyValidation.Create(() => Age < 11, "Age cannot be less than 11"),
                    PropertyValidation.Create(() => Age > 100, "Age cannot be more than 100")
                    );
            }
        }
    }
}
