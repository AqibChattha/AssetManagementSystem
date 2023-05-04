using AssetManagementSystem.Services;
using AssetManagementSystem.Services.ModelServices;
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
			try { BackupService.CheckBackup(); } catch (Exception) { }
            Application.Run(new LoginForm());
            try { BackupService.CheckBackup(); } catch (Exception) { }
        }
	}
}
