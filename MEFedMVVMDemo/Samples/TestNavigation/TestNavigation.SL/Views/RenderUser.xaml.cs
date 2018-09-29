using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MEFedMVVM.NavigationExtensions;

namespace TestNavigation.SL.Views
{
	[NavigationView("http://myApp/RenderUser")]
	public partial class RenderUser : UserControl
	{
		public RenderUser()
		{
			InitializeComponent();
		}
	}
}
