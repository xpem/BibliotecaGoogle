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



        public MainPage()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
        }

        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            ActivitySpinner.IsRunning = true;
            BtnBuscar.IsEnabled = false;
           Task.Run(() =>
            {
                string Busca = EntBusca.Text;
                Device.BeginInvokeOnMainThread(() => {
                    this.Navigation.PushAsync(new ListaLivros(Busca));
                    ActivitySpinner.IsRunning = false;
                    BtnBuscar.IsEnabled = true;
                });
            });
   
    
        }        
    }
}
