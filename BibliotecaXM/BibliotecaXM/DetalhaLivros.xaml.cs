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
        private static string IdBook, Key;
        //itens que são carregados de forma assincrona
        #region variaveis do bind.
        public string thumbnail, vTitle, vSubtitle, vDescription, vAuthors, vPageCount,
            vPublisher, vCategories, vStatus, vAvaliacao;
        public string Thumbnail
        {
            get => thumbnail;
            set
            {
                thumbnail = value; OnPropertyChanged();
            }
        }

        public string VTitle
        {
            get => vTitle;
            set
            {
                vTitle = value; OnPropertyChanged();
            }
        }

        public string VSubtitle
        {
            get => vSubtitle;
            set
            {
                vSubtitle = value; OnPropertyChanged();
            }
        }

        public string VDescription
        {
            get => vDescription;
            set
            {
                vDescription = value; OnPropertyChanged();
            }
        }
        public string VAuthors
        {
            get => vAuthors;
            set
            {
                vAuthors = value; OnPropertyChanged();
            }
        }

        public string VPageCount
        {
            get => vPageCount;
            set
            {
                vPageCount = value; OnPropertyChanged();
            }
        }

        public string VPublisher
        {
            get => vPublisher;
            set
            {
                vPublisher = value; OnPropertyChanged();
            }
        }

        public string VCategories
        {
            get => vCategories;
            set
            {
                vCategories = value; OnPropertyChanged();
            }
        }

        public string VStatus
        {
            get => vStatus;
            set
            {
                vStatus = value; OnPropertyChanged();
            }
        }

        public string VAvaliacao
        {
            get => vAvaliacao;
            set
            {
                vAvaliacao = value; OnPropertyChanged();
            }
        }
        #endregion

        private string VStatusOri { get; set; }
        private string VAvaliacaoOri { get; set; }

        /// <summary>
        /// seto com true caso já tenha uma avaliação
        /// </summary>
        private bool DessabilitaItensAlteracao { get; set; }

        public DetalhaLivros(string id)
        {
            IdBook = id;
            DessabilitaItensAlteracao = false;
            VStatus = VAvaliacao = "0";
            Key = null;

            BindingContext = this;

            InitializeComponent();

            ObservableCollection<string> BBTStatus = new ObservableCollection<string> { "Nenhum", "Vou ler", "Lendo", "Lido", "Interrompido" };

            PkrBiblioteca.ItemsSource = BBTStatus;

            this.Title = "Loading";

            //async 1
            CarregaBook(id);

            //async 2
            CarregaBookStatus(id);

            this.Title = "Done";

            AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = BtnConf.IsVisible = false;

        }

        private async void CarregaBook(string id)
        {
            Book book = await WsServiceBook.WsBook(id);

            Thumbnail = book.Thumbnail;
            VTitle = book.Title;
            VAuthors = book.Authors;
            VCategories = book.Categories;
            VPageCount = (book.PageCount += " páginas");
            VPublisher = book.Publisher;

            if (string.IsNullOrEmpty(book.Subtitle))
            {
                LblSubtitle.IsVisible = false;
            }
            else
                VSubtitle = book.Subtitle;

            VDescription = book.Description;
        }

        private async void CarregaBookStatus(string id)
        {
            BookStatus bookStatus = await BL.Services.FbBook.GetBookStatus(id);

            if (bookStatus != null)
            {
                Key = bookStatus.Key;
                VStatusOri = VStatus = bookStatus.Status.ToString();
                VAvaliacaoOri = VAvaliacao = bookStatus.Avaliacao.ToString();
                DessabilitaItensAlteracao = true;
                AvRSldr.IsVisible = PkrBiblioteca.IsEnabled = LblSdlrAvaliacao.IsEnabled = false;
                BtnConf.Text = "Alterar";
            }
            else
            {
                VStatus = VAvaliacao = "0";
                BtnConf.IsVisible = false;
            }
        }

        private void PkrBiblioteca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PkrBiblioteca.SelectedIndex == 0)
            {
                AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = false;
            }
            else
            {
                BtnConf.IsVisible = BtnConf.IsEnabled = true;
                AtribuicoesAvaliador();
            }
        }

        /// <summary>
        /// habilita o avaliador caso o pkr esteja setado como 'lido'
        /// </summary>
        private void AtribuicoesAvaliador()
        {
            if (PkrBiblioteca.SelectedIndex == 3)
            {
                AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = true;
            }
            else
            {
                AvRSldr.IsVisible = LblSdlrAvaliacao.IsVisible = false;
            }
        }

        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            if (DessabilitaItensAlteracao)
            {
                PkrBiblioteca.IsEnabled = LblSdlrAvaliacao.IsEnabled = true;

                AtribuicoesAvaliador();

                BtnConf.Text = "Confirmar";
                DessabilitaItensAlteracao = false;
            }
            else
            {

                this.Title = "Loading";
                BtnConf.IsEnabled = false;
                int avaliacao = 0;
                bool alterou = false;
                if (VAvaliacaoOri != VAvaliacao)
                {
                    alterou = true;
                }
                else if (VStatusOri != VStatus)
                    alterou = true;

                if (alterou)
                {
                    if (PkrBiblioteca.SelectedIndex == 3)
                    {
                        avaliacao = Convert.ToInt32(AvRSldr.Value);
                    }

                    if (string.IsNullOrEmpty(Key))
                    {
                        BL.Services.FbBook.AddBookStatus(IdBook, PkrBiblioteca.SelectedIndex, avaliacao);
                    }
                    else
                    {
                        if (PkrBiblioteca.SelectedIndex == 0)
                        {
                            BL.Services.FbBook.DeleteBookStatus(Key);
                        }
                        else
                            BL.Services.FbBook.UpdateBookStatus(Key, IdBook, PkrBiblioteca.SelectedIndex, avaliacao);
                    }

                    AvRSldr.IsVisible = BtnConf.IsVisible = false;
                    this.Title = "Done";
                }
                else
                {
                    DisplayAlert("Aviso", "Sem alterações", null, "Ok");
                    BtnConf.IsEnabled = true;
                }
            }
        }


    }
}