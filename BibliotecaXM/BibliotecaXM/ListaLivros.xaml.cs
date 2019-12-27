using BL.Models;
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
        private ObservableCollection<Book> Itenslista;
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


        BL.WsService ws = new BL.WsService();
        int TotalItems { get; set; }

        public ListaLivros (string Busca)
		{
			InitializeComponent ();
         
            int indice = 0;
            Itenslista = new ObservableCollection<Book>();
            CarregaLista(Busca, indice);
            LstLivros.ItemsSource = Itenslista;

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


        private async Task CarregaLista(string Busca,int indice)
        {
            isLoading = true;
            this.Title = "Loading";

            foreach(Book book in  ws.WsBusca(Busca, 0, "", indice))
            {
                book.PageCount += " páginas";
                Itenslista.Add(book);
            }
            
            this.Title = "Done";
            isLoading = false;
        }

	}
}