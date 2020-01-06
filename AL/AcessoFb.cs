using ML;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL
{
    public class AcessoFb
    {
       public static FirebaseClient firebase = new FirebaseClient("https://mylibrary-20954.firebaseio.com/");

        public async static void AddUserBookStatus(int vIdUsuario,string vIdBook,int vStatus,int vAvaliacao )
        {
            await firebase
              .Child("BookStatus")
              .PostAsync(new BookStatus() { IdUsuario = vIdUsuario, IdBook= vIdBook, Status = vStatus, Avaliacao=vAvaliacao });
        }

        public async static Task<BookStatus> GetBookStatus(int vIdUsuario,string vIdBook)
        {

            List<BookStatus> allBookStatus = await GetAllUserBookStatus();
            await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>();
            return allBookStatus.Where(a => a.IdUsuario == vIdUsuario && a.IdBook == vIdBook).FirstOrDefault();
        }

        public async static Task<List<BookStatus>> GetAllUserBookStatus()
        {
            return (await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Select(item => new BookStatus
              {
                  Key = item.Key,
                  IdBook = item.Object.IdBook,
                  IdUsuario = item.Object.IdUsuario,
                  Status = item.Object.Status,
                  Avaliacao = item.Object.Avaliacao
              }).ToList();
        }

        public async static Task<List<BookStatus>> GetUserBookStatusByStatus(int vStatus,int vIdusuario)
        {
            return (await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Where(a => a.Object.IdUsuario == vIdusuario && a.Object.Status == vStatus).Select(item => new BookStatus
              {
                  Key = item.Key,
                  IdBook = item.Object.IdBook,
                  IdUsuario = item.Object.IdUsuario,
                  Status = item.Object.Status,
                  Avaliacao = item.Object.Avaliacao
              }).ToList();
        }

        public async static void UpdateBookStatus(string vKey, int vIdUsuario, string vIdBook, int vStatus, int vAvaliacao)
        {
            var toUpdateBookStatus = (await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Where(a => a.Key == vKey).FirstOrDefault();

            await firebase
              .Child("BookStatus")
              .Child(toUpdateBookStatus.Key)
              .PutAsync(new BookStatus() { IdUsuario = vIdUsuario, IdBook = vIdBook, Status = vStatus, Avaliacao = vAvaliacao });
        }

    }



}
