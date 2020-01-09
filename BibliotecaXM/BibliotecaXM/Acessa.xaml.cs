﻿using System;
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
                if (EntSenha.Text.Length > 3)
                {
                    BtnCadAcesso.IsEnabled = false;
                    bool resp = false;
                    Task.Run(async () => resp = await BL.Services.FbLogin.RecuperaUsuario(EntNomeAcesso.Text, EntSenha.Text)).Wait();

                    if (resp)
                    {
                        Application.Current.MainPage = new MainPage();
                        Application.Current.MainPage = new NavigationPage(new MainPage())
                        {
                            BarBackgroundColor = Color.FromHex("#301810"),
                            BarTextColor = Color.White
                        };
                    }else
                    {
                        DisplayAlert("Aviso", "Usuário/senha incorretos", null, "Ok");
                    }
                    BtnCadAcesso.IsEnabled = true;
                }
                else
                {
                    DisplayAlert("Aviso", "Digite sua senha", null, "Ok");
                }
            }
        }

        private void BtnCadAcesso_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new CadastraUsuario());
        }
    }
}