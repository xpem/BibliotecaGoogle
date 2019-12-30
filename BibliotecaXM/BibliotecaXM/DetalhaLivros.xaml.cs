using BL;
using BL.Models;
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
    public partial class DetalhaLivros : ContentPage
    {

        WsServiceBook ws = new WsServiceBook();

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
        #endregion

        public DetalhaLivros(string id)
        {
            Book book = new Book();

            BindingContext = this;

            Task.Run(async () =>
            {
                book = await ws.WsBook(id);
                Thumbnail = book.Thumbnail;
                VTitle = book.Title;
                VAuthors = book.Authors;
                VCategories = book.Categories;
                VPageCount = (book.PageCount += "páginas");
                VPublisher = book.Publisher;
                VSubtitle = book.Subtitle;
                VDescription = book.Description;
            }).Wait();

            InitializeComponent();

        }




        //Book book = new Book();
        //public DetalhaLivros(string id)
        //{


        //    BindingContext = this;

        //    Initialization = InitializeAsync(id);

        //    InitializeComponent();

        //}

        //public Task Initialization { get; private set; }

        //private async Task InitializeAsync(string id)
        //{
        //    book = await ws.WsBook(id);
        //    Thumbnail = book.Thumbnail;
        //    VTitle = book.Title;
        //}


    }
}