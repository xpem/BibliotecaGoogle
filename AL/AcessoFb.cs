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
        public static FirebaseClient firebase = new FirebaseClient("<link>");

        #region bookstatus
        public async static void AddUserBookStatus(string vKeyUsuario, string vIdBook, int vStatus, int vAvaliacao)
        {
            await firebase
              .Child("BookStatus")
              .PostAsync(new BookStatus() { KeyUsuario = vKeyUsuario, IdBook = vIdBook, Status = vStatus, Avaliacao = vAvaliacao });
        }

        public async static Task<BookStatus> GetBookStatus(string vKeyUsuario, string vIdBook)
        {
            await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>();
            return (await firebase.Child("BookStatus").OnceAsync<BookStatus>()).Where(a => a.Object.KeyUsuario == vKeyUsuario && a.Object.IdBook == vIdBook).Select(item => new BookStatus
            {
                Key = item.Key,
                IdBook = item.Object.IdBook,
                KeyUsuario = item.Object.KeyUsuario,
                Status = item.Object.Status,
                Avaliacao = item.Object.Avaliacao
            }).FirstOrDefault();
        }

        public async static Task<List<BookStatus>> GetUserBookStatusByStatus(int vStatus, string vKeyuser)
        {
            return (await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Where(a => a.Object.KeyUsuario == vKeyuser && a.Object.Status == vStatus).Select(item => new BookStatus
              {
                  Key = item.Key,
                  IdBook = item.Object.IdBook,
                  KeyUsuario = item.Object.KeyUsuario,
                  Status = item.Object.Status,
                  Avaliacao = item.Object.Avaliacao
              }).ToList();
        }

        public async static void UpdateBookStatus(string vKey, string vKeyUsuario, string vIdBook, int vStatus, int vAvaliacao)
        {
            var toUpdateBookStatus = (await firebase
              .Child("BookStatus")
              .OnceAsync<BookStatus>()).Where(a => a.Key == vKey).FirstOrDefault();

            await firebase
              .Child("BookStatus")
              .Child(toUpdateBookStatus.Key)
              .PutAsync(new BookStatus() { KeyUsuario = vKeyUsuario, IdBook = vIdBook, Status = vStatus, Avaliacao = vAvaliacao });
        }

        public async static void DeleteBookStatus(string vKey)
        {
            await firebase.Child("BookStatus").Child(vKey).DeleteAsync();
        }

        #endregion

       


    }



}
