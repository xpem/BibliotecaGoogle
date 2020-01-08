using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BibliotecaXM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Acessa : ContentPage
    {
        public Acessa()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EntNomeAcesso.Text) && !string.IsNullOrEmpty(EntSenha.Text))
            {
                if (EntSenha.Text.Length > 4)
                {
                    BL.Services.SqLiteLogin.CadatraAcesso("1", "Emanuel Martins");


                    Application.Current.MainPage = new MainPage();
                    Application.Current.MainPage = new NavigationPage(new MainPage())
                    {
                        BarBackgroundColor = Color.FromHex("#301810"),
                        BarTextColor = Color.White
                    };
                }
            }
        }
    }
}