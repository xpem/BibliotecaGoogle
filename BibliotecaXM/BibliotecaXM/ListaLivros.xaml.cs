using ML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BibliotecaXM
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaLivros : ContentPage
    {
        public ObservableCollection<Book> Itenslista;
        bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }


        public ListaLivros(string Busca)
        {
            BindingContext = this;
            int indice = 0;
            Itenslista = new ObservableCollection<Book>();

            InitializeComponent();

            LstLivros.ItemsSource = Itenslista;

           CarregaLista(Busca, indice);
           CarregaFuncaoSelecao(Busca, indice);
        }

        private async void CarregaLista(string Busca, int indice)
        {
            isLoading = true;
            this.Title = "Loading";

            List<Book> lista = await BL.WsServiceLista.WsBusca(Busca, 0, "", indice);

            if (lista.Count == 0)
            {
                this.Title = "Done";
                isLoading = false;
                await DisplayAlert("Aviso", "Sem retorno para a busca", null, "Ok");
            }
            else
            {

                foreach (Book book in lista)
                {
                    if (!string.IsNullOrEmpty(book.PageCount))
                    {
                        book.PageCount += " páginas";
                    }

                    Itenslista.Add(book);
                }
                this.Title = "Done";
                isLoading = false;
            }
            
        }

        private void CarregaFuncaoSelecao(string Busca, int indice)
        {
            LstLivros.ItemAppearing += (sender, e) =>
            {
                if (isLoading || Itenslista.Count == 0)
                    return;

                //hit bottom!
                if (e.Item == Itenslista[Itenslista.Count - 1])
                {
                    indice += 10;
                    CarregaLista(Busca, indice);
                }
            };
        }

        private void LstLivros_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = (Book)e.SelectedItem;
                DetalhaLivros detalhaLivros = new DetalhaLivros(book.Id);
                Navigation.PushAsync(detalhaLivros);
                LstLivros.SelectedItem = null;
            }

        }
    }
}