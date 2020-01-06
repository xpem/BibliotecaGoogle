using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BL
{
    public class DbLite
    {
        static SQLiteAsyncConnection _database;

        public static async void CriaDb()
        {

            _database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bibliot.db3"));
            // _database.CreateTableAsync<Note>().Wait();

            await _database.ExecuteAsync("create table if not exists acessdd (id integer primary key autoincrement, codigo text, nick text)");
        }


        public static async void Save(string codigo, string nick)
        {
            await _database.ExecuteAsync($"insert into acessdd (codigo,nick) values ('{codigo}','{nick}')");
        }
    }


}
