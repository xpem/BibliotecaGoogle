using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
   public class FbBookshelf
    {
        public static int Total { get; set; }

        public async static Task<Bookshelf.Totais> GetBookshelfTotais()
        {
            ML.User login = BL.Services.SqLiteLogin.RecAcesso();
            return await AL.ABookshelf.GetBookshelfTotais(login.Key);
        }

        public async static Task<List<BookStatus>> GetUserBookStatusByStatus(int Status,int index,string LoginCodigo)
        {

            List<BookStatus> lista = await AL.AcessoFb.GetUserBookStatusByStatus(Status, LoginCodigo);
            Total = lista.Count();

            if (Total > (index + 10))
             return lista.GetRange(index, 10);
            else
                return lista.GetRange(index, (Total - index));
        }
    }
}
