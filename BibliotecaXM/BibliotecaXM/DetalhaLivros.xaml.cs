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
    public partial class DetalhaLivros : ContentPage
    {

        WsServiceBook ws = new WsServiceBook();
        private static string IdBook;
        private static string Key;

        //itens que são carregados de forma assincrona
        #region variaveis do bind.
        public string thumbnail;
        public string Thumbnail
        {
            get => thumbnail;
            set
            {
                thumbnail = value; OnPropertyChanged();
            }
        }

        private string vTitle;
        public string VTitle
        {
            get => vTitle;
            set
            {
                vTitle = value; OnPropertyChanged();
            }
        }
        private string vSubtitle;
        public string VSubtitle
        {
            get => vSubtitle;
            set
            {
                vSubtitle = value; OnPropertyChanged();
            }
        }

        private string vDescription;
        public string VDescription
        {
            get => vDescription;
            set
            {
                vDescription = value; OnPropertyChanged();
            }
        }

        private string vAuthors;
        public string VAuthors
        {
            get => vAuthors;
            set
            {
                vAuthors = value; OnPropertyChanged();
            }
        }

        private string vPageCount;
        public string VPageCount
        {
            get => vPageCount;
            set
            {
                vPageCount = value; OnPropertyChanged();
            }
        }

        private string vPublisher;
        public string VPublisher
        {
            get => vPublisher;
            set
            {
                vPublisher = value; OnPropertyChanged();
            }
        }

        private string vCategories;
        public string VCategories
        {
            get => vCategories;
            set
            {
                vCategories = value; OnPropertyChanged();
            }
        }
        private string vStatus;
        public string VStatus
        {
            get => vStatus;
            set
            {
                vStatus = value; OnPropertyChanged();
            }
        }
        private string vAvaliacao;
        public string VAvaliacao
        {
            get => vAvaliacao;
            set
            {
                vAvaliacao = value; OnPropertyChanged();
            }
        }
        #endregion

        public DetalhaLivros(string id)
        {
            Book book = new Book();
            IdBook = id;
            VStatus = VAvaliacao = "0";
            Key = null;

            BindingContext = this;

            //1
            Task.Run(async () =>
            {
                this.Title = "Loading";
                book = await ws.WsBook(id);

                Thumbnail = book.Thumbnail;
                VTitle = book.Title;
                VAuthors = book.Authors;
                VCategories = book.Categories;
                VPageCount = (book.PageCount += " páginas");
                VPublisher = book.Publisher;
                VSubtitle = book.Subtitle;
                VDescription = book.Description;
                this.Title = "Done";
            }).Wait();

            InitializeComponent();

            ObservableCollection<string> BBTStatus = new ObservableCollection<string> { "Nenhum", "Favoritos", "Vou ler", "Lendo", "Lido" };

            PkrBiblioteca.ItemsSource = BBTStatus;

            //2
            Task.Run(async () =>
            {
                this.Title = "Loading";
                BookStatus bookStatus = new BookStatus();

                bookStatus = await BL.Services.FbServices.GetBookStatus(id);

                if (bookStatus != null)
                {
                    Key = bookStatus.Key;
                    VStatus = bookStatus.Status.ToString();
                    VAvaliacao = bookStatus.Avaliacao.ToString();
                    BtnBuscar.Text = "Alterar";
                }
                else
                {
                    VStatus = VAvaliacao = "0";
                }
                this.Title = "Done";

            }).Wait();


            //PkrBiblioteca.SelectedIndex = 0;

            AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = BtnBuscar.IsVisible = false;

        }

        private void PkrBiblioteca_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = PkrBiblioteca.SelectedIndex;

            if (PkrBiblioteca.SelectedIndex == 0)
            {
                BtnBuscar.IsVisible = false;
            }
            else
            {
                BtnBuscar.IsVisible = true;

                if (PkrBiblioteca.SelectedIndex == 4)
                {
                    AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = true;
                }
                else
                {
                    AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = false;
                }
            }
        }

        private async void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            this.Title = "Loading";
            BtnBuscar.IsEnabled = false;
            int avaliacao = 0;

            if (PkrBiblioteca.SelectedIndex == 4)
            {
                avaliacao = Convert.ToInt32(AvRSldr.Value);
            }

            if (string.IsNullOrEmpty(Key))
            {
                BL.Services.FbServices.AddBookStatus(IdBook, PkrBiblioteca.SelectedIndex, avaliacao);
            }
            else
            {
                BL.Services.FbServices.UpdateBookStatus(Key, IdBook, PkrBiblioteca.SelectedIndex, avaliacao);
            }
            this.Title = "Done";
            
        }
    }
}