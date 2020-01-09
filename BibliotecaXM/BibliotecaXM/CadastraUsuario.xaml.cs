using ML;
using Plugin.Connectivity;
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
    public partial class CadastraUsuario : ContentPage
    {
        public CadastraUsuario()
        {
            InitializeComponent();
        }


        private void BtnCadastrar_Clicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                 DisplayAlert("Aviso", "Sem conexão com a internet", null, "Ok");
                return;
            }

            if (ValidaCadastro())
            {
                User user = new User
                {
                    Nome = EntNome.Text,
                    LoginNome = EntNomeAcesso.Text,
                    Email = EntEmail.Text,
                    Senha = BL.Services.FbLogin.CPEncrypt(EntSenha.Text, EntSenha.Text.Length)
                };
                //
                BL.Services.FbLogin.CadastraUsuario(user);
                DisplayAlert("Aviso", "Usuário cadastrado", null, "Ok");
                BtnCadastrar.IsEnabled = false;
            }
        }


        private bool ValidaCadastro()
        {
            bool ValCampos = true;
            if (string.IsNullOrEmpty(EntNome.Text))
            {
                ValCampos = false;
            }
            if (string.IsNullOrEmpty(EntEmail.Text))
            {
                ValCampos = false;
            }
            else if (!BL.Services.FbLogin.Valida_email(EntEmail.Text))
            {
                DisplayAlert("Aviso", "Digite um email válido", null, "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(EntNomeAcesso.Text))
            {
                ValCampos = false;
            }
            if (string.IsNullOrEmpty(EntSenha.Text))
            {
                ValCampos = false;
            }
            else if (EntSenha.Text.Length < 4)
            {
                ValCampos = false;
            }
            if (string.IsNullOrEmpty(EntConfSenha.Text))
            {
                ValCampos = false;
            }
            if (EntConfSenha.Text != EntSenha.Text)
            {
                ValCampos = false;
            }

            if (!ValCampos)
            {
                DisplayAlert("Aviso", "Preencha os campos e confirme a senha corretamente", null, "Ok");
            }
            else
            {
                ValCampos = BL.Services.FbLogin.VerificaCadUsuario(EntNome.Text, EntEmail.Text);
                if (!ValCampos)
                {
                    DisplayAlert("Aviso", "Nome de Acesso/Email já cadastrados", null, "Ok");
                }
            }
            return ValCampos;
        }

        private void BtnVoltar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}