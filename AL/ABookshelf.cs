using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL
{
   public class ABookshelf
    {

        /// <summary>
        /// Recupera o total de cada status de livro do usuario atual
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public async static Task<Bookshelf.Totais> GetBookshelfTotais(int IdUsuario)
        {
            Bookshelf.Totais BTotais = new Bookshelf.Totais();

            //obtenho a lista geral(pois não consegui obter uma lista de counts independentes)
            List<BookStatus> lista = ((await AcessoFb.firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario).Select(obj => new BookStatus{ Status = obj.Object.Status})).ToList();

            //aqui filtro na lista counts por status
            BTotais.VouLer = lista.Where(a => a.Status == 1).Count();         
            BTotais.Lendo = lista.Where(a => a.Status == 2).Count();
            BTotais.Lido = lista.Where(a => a.Status == 3).Count();
            BTotais.Interrompido = lista.Where(a => a.Status == 4).Count();
            // .Child("BookStatus")
            // .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario && a.Object.Status == 2).Count();
            //BTotais.Lido = (await AcessoFb.firebase
            // .Child("BookStatus")
            // .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario && a.Object.Status == 3).Count();
            //BTotais.Interrompido = (await AcessoFb.firebase
            //.Child("BookStatus")
            //.OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario && a.Object.Status == 4).Count();

            return BTotais;
        }
    }
}
