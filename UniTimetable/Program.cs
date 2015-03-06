#region

using System;
using System.IO;
using System.Windows.Forms;
using UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects;
using UniTimetable.ViewControllers;

#endregion

namespace UniTimetable
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}