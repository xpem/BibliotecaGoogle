using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class SqLiteLogin
    {

        public static void CriaBD() =>
            AL.ASqLite.CriaDb();

        public static bool VerificaAcesso()
        {
            List<ML.User> res = new List<ML.User>();
            Task.Run(async () => res = await AL.ASqLite.RecAcesso()).Wait();

            if (res.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public static void CadatraAcesso(string id, string login)
        {
            AL.ASqLite.CadastraAcesso(id, login);
        }

        public static ML.User RecAcesso()
        {
            List<User> res = new List<ML.User>();
            Task.Run(async () => res = await AL.ASqLite.RecAcesso()).Wait();
            return res[0];
        }

        public static void DelAcesso() => AL.ASqLite.DelAcesso();

    }
}
