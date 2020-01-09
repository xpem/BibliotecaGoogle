using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BibliotecaXM
{
    public partial class App : Application
    {
        public App()
        {

            BL.Services.SqLiteLogin.CriaBD();

            InitializeComponent();

            if (BL.Services.SqLiteLogin.VerificaAcesso())
            {
                //MainPage = new Acessa();
                MainPage = new MainPage();
                MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#301810"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                MainPage = new Acessa();
            }

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
