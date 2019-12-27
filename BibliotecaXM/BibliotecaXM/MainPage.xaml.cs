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
    public partial class MainPage : CarouselPage
    {

        public MainPage()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
        }

        private async void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            ActivitySpinner.IsRunning = true;
            string Busca = EntBusca.Text;
            ListaLivros page = new ListaLivros(Busca);
            await  this.Navigation.PushAsync(page);
            ActivitySpinner.IsRunning = false;
        }
    }
}
