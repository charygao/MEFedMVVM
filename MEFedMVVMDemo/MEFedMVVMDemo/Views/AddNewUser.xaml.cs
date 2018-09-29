using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MEFedMVVM.NavigationExtensions;

namespace MEFedMVVMDemo.Views
{
    /// <summary>
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
	[NavigationView("http://meffeddemo/addUser")]
    public partial class AddNewUser : UserControl
    {
        public AddNewUser()
        {
            InitializeComponent();
        }
    }
}
