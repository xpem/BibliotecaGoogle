using BL;
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
    public partial class ListaBookshelf : ContentPage
    {
        public ObservableCollection<Book> ItensBookLista;
        bool isLoading;
        private int indice;

        public ListaBookshelf(int Status)
        {
            BindingContext = this;
            indice = 0;
            ItensBookLista = new ObservableCollection<Book>();
            InitializeComponent();

            LstLivros.ItemsSource = ItensBookLista;

            CarregaLista(Status);
            CarregaFuncaoSelecao(Status);
        }
        private async void CarregaLista(int status)
        {
            this.Title = "Loading";
            isLoading = true;
            Book book;

            foreach (BookStatus bookStatus in (await BL.Services.FbBookshelf.GetUserBookStatusByStatus(status, indice)))
            {
                book = await WsServiceBook.WsBook(bookStatus.IdBook);
                switch (bookStatus.Status)
                {
                    case 1:
                        book.Status = "Vou Ler";
                        book.StatusColor = "#003d33";
                        break;
                    case 2:
                        book.Status = "Lendo";
                        book.StatusColor = "#00695c";
                        break;
                    case 3:
                        book.Status = "Lido";
                        book.Avaliacao = $"Avaliação: {bookStatus.Avaliacao} de 5";
                        book.StatusColor = "#003d33";
                        break;
                }

                ItensBookLista.Add(book);
            }

            this.Title = "Done";
            isLoading = false;
        }

        private void CarregaFuncaoSelecao(int Status)
        {

            LstLivros.ItemAppearing += (sender, e) =>
            {
                if (BL.Services.FbBookshelf.Total > 10)
                {
                    if (isLoading || ItensBookLista.Count == 0)
                        return;

                    //hit bottom!
                    if (e.Item == ItensBookLista[ItensBookLista.Count - 1])
                    {
                        indice += 10;
                        if (indice < BL.Services.FbBookshelf.Total)
                            CarregaLista(Status);
                    }
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