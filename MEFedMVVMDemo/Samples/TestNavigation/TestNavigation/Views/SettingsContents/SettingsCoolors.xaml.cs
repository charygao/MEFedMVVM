﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace TestNavigation.Views.SettingsContents
{
	/// <summary>
	/// Interaction logic for SettingsCoolors.xaml
	/// </summary>
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[NavigationView("http://myApp/settingsColors")]
	public partial class SettingsCoolors : UserControl
	{
		public SettingsCoolors()
		{
			InitializeComponent();
		}
	}
}
