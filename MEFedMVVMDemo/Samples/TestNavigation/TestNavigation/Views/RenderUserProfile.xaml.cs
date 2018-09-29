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

namespace TestNavigation.Views
{
	/// <summary>
	/// Interaction logic for RenderUserProfile.xaml
	/// </summary>
	[NavigationView("http://myApp/RenderUser")]
	public partial class RenderUserProfile : UserControl
	{
		public RenderUserProfile()
		{
			InitializeComponent();
		}
	}
}
