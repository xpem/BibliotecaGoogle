﻿using System;
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

        private int vVouLer, vLendo, vLido;

        public int VVouLer { get => vVouLer; set { vVouLer = value; OnPropertyChanged(); } }

        public int VLendo { get => vLendo; set { vLendo = value; OnPropertyChanged(); } }

        public int VLido { get => vLido; set { vLido = value; OnPropertyChanged(); } }


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

        public async void CarregaBookshelfTotais()
        {
            ML.Bookshelf.Totais totais = await BL.Services.FbBookshelf.GetBookshelfTotais();
            VVouLer = totais.VouLer;
            VLendo = totais.Lendo;
            VLido = totais.Lidos;
        }

        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            BL.Services.FbBook fbServices = new BL.Services.FbBook();

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
