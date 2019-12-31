using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BibliotecaXM
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        bool cornerRadius;

        public bool CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;
                OnPropertyChanged();
            }
        }        

        public MainPage()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
        }

        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            BL.Services.FbServices fbServices = new BL.Services.FbServices();
            
            if (!string.IsNullOrEmpty(EntBusca.Text))
            {
                ActivitySpinner.IsRunning = true;
                BtnBuscar.IsEnabled = false;
                Task.Run(() =>
                 {
                     string Busca = EntBusca.Text;
                     Device.BeginInvokeOnMainThread(() =>
                     {
                         this.Navigation.PushAsync(new ListaLivros(Busca));
                         ActivitySpinner.IsRunning = false;
                         BtnBuscar.IsEnabled = true;
                     });
                 });
            }
            else
            {
                DisplayAlert("Aviso", "Digite um texto para a busca", null, "Ok");
            }

        }        
    }
}
