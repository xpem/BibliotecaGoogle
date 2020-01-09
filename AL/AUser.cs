using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;

namespace AL
{
    public class AUser
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vLoginNome"></param>
        /// <param name="vEmail"></param>
        /// <returns>false para duplicado, true para ok</returns>
        public async static Task<bool> VerificaCadUsuario(string vLoginNome, string vEmail)
        {
            if ((await AcessoFb.firebase
            .Child("User")
            .OnceAsync<User>()).Select(item => new User
            {
                Key = item.Key,
            }).Where(a => (a.LoginNome == vLoginNome && a.Email == vEmail) || (a.LoginNome == vLoginNome || a.Email == vEmail)).ToList().Count > 0)
                return false;
            else return true;
        }

        public async static Task<List<User>> RecLogin(string vLoginNome, string vSenha)
        {
            return (await AcessoFb.firebase.Child("User").OnceAsync<User>()).Where(a => (a.Object.LoginNome == vLoginNome && a.Object.Senha == vSenha)).Select(item => new User
            {
                Key = item.Key,
                LoginNome = item.Object.LoginNome,
            }).ToList();
        }

        public async static void CadastraUsuario(User user)
        {
            await AcessoFb.firebase.Child("User").PostAsync(new User()
            {
                Nome = user.Nome,
                LoginNome = user.LoginNome,
                Email = user.Email,
                Senha = user.Senha
            });
        }
    }
}
