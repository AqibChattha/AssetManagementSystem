using AssetManagementSystem.Services;
using AssetManagementSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			DatabaseHelper.InitializeDatabase();
			Application.Run(new LoginForm());
		}
	}
}
