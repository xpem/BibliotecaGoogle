using BL;
using ML;
using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BibliotecaXM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaBookshelf : ContentPage
    {
        public ObservableCollection<Book> ItensBookLista;

        private bool isLoading;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(); } }

        private int indice;

        /// <summary>
        /// status de livro repassado da Main.
        /// </summary>
        private int StatusBookIndex { get; set; }

        public ListaBookshelf(int Status)
        {
            BindingContext = this;
            StatusBookIndex = Status;
            indice = 0;
            ItensBookLista = new ObservableCollection<Book>();
            InitializeComponent();

            LstLivros.ItemsSource = ItensBookLista;

            CarregaLista();
            CarregaFuncaoSelecao();
        }
        private async void CarregaLista()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Aviso", "Sem conexão com a internet", null, "Ok");
                return;
            }

            this.Title = "Carregando lista...";
            IsLoading = true;
            Book book;
            ML.User login = BL.Services.SqLiteLogin.RecAcesso();

            foreach (BookStatus bookStatus in (await BL.Services.FbBookshelf.GetUserBookStatusByStatus(StatusBookIndex, indice, login.Key)))
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
                    case 4:
                        book.Status = "Interrompido";
                        book.StatusColor = "#00251";
                        break;
                }
                ItensBookLista.Add(book);
            }

            this.Title = "Estante";
            IsLoading = false;
        }


        private async void CarregaFuncaoSelecao()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Aviso", "Sem conexão com a internet", null, "Ok");
                return;
            }

            LstLivros.ItemAppearing += (sender, e) =>
            {
                if (BL.Services.FbBookshelf.Total > 10)
                {
                    if (IsLoading || ItensBookLista.Count == 0)
                        return;

                    //hit bottom!
                    if (e.Item == ItensBookLista[ItensBookLista.Count - 1])
                    {
                        indice += 10;
                        if (indice < BL.Services.FbBookshelf.Total)
                            CarregaLista();
                    }
                }
            };
        }

        private async void LstLivros_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = (Book)e.SelectedItem;
                DetalhaLivros detalhaLivros = new DetalhaLivros(book.Id);

                //atualiza o item com o retorno à esta view
                detalhaLivros.Disappearing += async (sender2, e2) =>
                {
                    BookStatus novobookStatus = await CarregaBookStatus(book);

                    if (novobookStatus != null && (novobookStatus.Status == 3) && (StatusBookIndex == novobookStatus.Status))
                    {
                        //atualiza a avaliação
                        int index = ItensBookLista.IndexOf(book);
                        book.Avaliacao = $"Avaliação: {novobookStatus.Avaliacao} de 5";
                        ItensBookLista[index] = book;
                    }
                    else if (StatusBookIndex != novobookStatus.Status)
                        ItensBookLista.RemoveAt(ItensBookLista.IndexOf(book));
                };

                await Navigation.PushAsync(detalhaLivros);
                LstLivros.SelectedItem = null;
            }
        }

        private async Task<BookStatus> CarregaBookStatus(Book book)
        {
            BookStatus bookStatus = await BL.Services.FbBook.GetBookStatus(book.Id);

            if (bookStatus != null)
            {
                return bookStatus;
            }
            else return null;

        }
    }

}