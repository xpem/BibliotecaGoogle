﻿using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BL.Services
{
    public class FbLogin
    {
        public static bool VerificaCadUsuario(string LoginNome, string Email)
        {
            bool ret = false;
            Task.Run(async () => ret = await AL.AUser.VerificaCadUsuario(LoginNome, Email)).Wait();
            return ret;
        }


        public static void CadastraUsuario(User user) => AL.AUser.CadastraUsuario(user);

        public async static Task<bool> RecuperaUsuario(string LoginNome, string Senha)
        {
            Senha = CPEncrypt(Senha, Senha.Length);
            List<User> Lstuser = await AL.AUser.RecLogin(LoginNome, Senha);
            if (Lstuser.Count > 0)
            {
                BL.Services.SqLiteLogin.CadatraAcesso(Lstuser[0].Key, Lstuser[0].Nome);
                return true;
            }
            else return false;
        }

        /// <returns>true para válido</returns>
        public static bool Valida_email(string email)
        {
            if (Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string CPEncrypt(string input, int key)
        {
            StringBuilder result = new StringBuilder();
            char[] charArray;

            for (int i = key; i >= 1; i--)
                result.Append(input.Substring(i - 1, 1));

            charArray = result.ToString().ToCharArray();
            result = new StringBuilder();

            for (int i = 0; i < key; i++)
                result.Append(Convert.ToChar(charArray[i] + (i + 1) * 2));

            return result.ToString();
        }
    }
}
