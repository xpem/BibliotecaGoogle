using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BibliotecaXM
{
    public partial class App : Application
    {
        public App()
        {
            //BL.DbLite.CriaDb();

            InitializeComponent();

            //MainPage = new Acessa();
            MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#301810"),
                BarTextColor = Color.White
            };

            InitializeComponent();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
