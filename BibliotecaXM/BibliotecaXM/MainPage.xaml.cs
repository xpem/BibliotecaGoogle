using Plugin.Connectivity;
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
        private bool cornerRadius;

        public bool CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;
                OnPropertyChanged();
            }
        }

        private int vVouLer, vLendo, vLido, vInterrompido;

        public int VVouLer { get => vVouLer; set { vVouLer = value; OnPropertyChanged(); } }

        public int VLendo { get => vLendo; set { vLendo = value; OnPropertyChanged(); } }

        public int VLido { get => vLido; set { vLido = value; OnPropertyChanged(); } }

        public int VInterrompido { get => vInterrompido; set { vInterrompido = value; OnPropertyChanged(); } }

        public MainPage()
        {
            BindingContext = this;
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregaBookshelfTotais();
        }

        private void BtnVouLer_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ListaBookshelf(1));
        }

        private void BtnLidos_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ListaBookshelf(3));
        }

        private void BtnLendo_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ListaBookshelf(2));
        }

        private void TbiSair_Clicked(object sender, EventArgs e)
        {
            BL.Services.SqLiteLogin.DelAcesso();
            Acessa pag = new Acessa();
            Application.Current.MainPage = new Acessa();
            Navigation.PushModalAsync(pag);
        }

        private void BtnInterrompido_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ListaBookshelf(4));
        }

        public async void CarregaBookshelfTotais()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Aviso", "Sem conexão com a internet", null, "Ok");
                return;
            }
            ML.Bookshelf.Totais totais = await BL.Services.FbBookshelf.GetBookshelfTotais();
                VVouLer = totais.VouLer;
                VLendo = totais.Lendo;
                VLido = totais.Lido;
                VInterrompido = totais.Interrompido;
            
        }

        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EntBusca.Text))
            {
                BtnBuscar.IsEnabled = false;
                string Busca = EntBusca.Text;
                this.Navigation.PushAsync(new ListaLivros(Busca));
                BtnBuscar.IsEnabled = true;
            }
            else
            {
                DisplayAlert("Aviso", "Digite um texto para a busca", null, "Ok");
            }
        }
    }
}
