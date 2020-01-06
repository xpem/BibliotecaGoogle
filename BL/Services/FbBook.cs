using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class FbBook
    {
        public async static void AddBookStatus(string IdBook, int Status, int Avaliacao)
        {
            //fixei o usuario como teste
            await Task.Run(() => AL.AcessoFb.AddUserBookStatus(1, IdBook, Status, Avaliacao));
        }

        public async static void UpdateBookStatus(string Key, string IdBook, int Status, int Avaliacao)
        {
         await Task.Run(() => AL.AcessoFb.UpdateBookStatus(Key, 1, IdBook, Status, Avaliacao));
        }

        public async static Task<BookStatus> GetBookStatus(string IdBook)
        {
            return await AL.AcessoFb.GetBookStatus(1, IdBook);
        }


    }
}
