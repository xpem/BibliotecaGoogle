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
            ML.User login = BL.Services.SqLiteLogin.RecAcesso();
            await Task.Run(() => AL.AcessoFb.AddUserBookStatus(login.Key, IdBook, Status, Avaliacao));
        }

        public async static void UpdateBookStatus(string Key, string IdBook, int Status, int Avaliacao)
        {
            ML.User login = BL.Services.SqLiteLogin.RecAcesso();
            await Task.Run(() => AL.AcessoFb.UpdateBookStatus(Key, login.Key, IdBook, Status, Avaliacao));
        }

        public async static Task<BookStatus> GetBookStatus(string IdBook)
        {
            ML.User login = BL.Services.SqLiteLogin.RecAcesso();
            return await AL.AcessoFb.GetBookStatus(login.Key, IdBook);
        }

        public async static void DeleteBookStatus(string Key)
        {
            await Task.Run(() => AL.AcessoFb.DeleteBookStatus(Key));
        }


    }
}
