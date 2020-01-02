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
            BTotais.VouLer = (await AcessoFb.firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario && a.Object.Status == 1).Count();
            BTotais.Lendo = (await AcessoFb.firebase
             .Child("BookStatus")
             .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario && a.Object.Status == 2).Count();
            BTotais.Lidos = (await AcessoFb.firebase
             .Child("BookStatus")
             .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == IdUsuario && a.Object.Status == 3).Count();

            return BTotais;
        }
    }
}
