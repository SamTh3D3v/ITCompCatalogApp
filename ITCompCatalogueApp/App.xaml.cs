using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace ITCompCatalogueApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //static App()
        //{
            
        //}
        public App()
            : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            DispatcherHelper.Initialize();
        }
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            MessageBox.Show("Something went wrong > Message :" + errorMessage);
            e.Handled = true;
        }

    }
}
