using ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WsServiceBook
    {
        public async Task<Book> WsBook(string id)
        {
            string urlBusca = $"https://www.googleapis.com/books/v1/volumes/{id}?key=AIzaSyAiqcLMIbWfBuTJCPA6AuEa-mUcPiZXS4E";
            Book livro = new Book();

            try
            {
                using (var http = new HttpClient())
                {
                    var requisicao = new HttpRequestMessage();
                    requisicao.RequestUri = new Uri(urlBusca.ToString());
                    requisicao.Method = HttpMethod.Get;
                    var resposta = await http.SendAsync(requisicao);
                    var conteudo = await resposta.Content.ReadAsStringAsync();
                  livro =  JsonBook(conteudo.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return livro;
        }

        public Book JsonBook(string json)
        {
            dynamic array = JsonConvert.DeserializeObject(json);

            //variaveis utilizadas na criação da lista
            Book livro = new Book();
            StringBuilder strbdrarrays = new StringBuilder();
            IList<string> lstAuthors;
            IList<string> lstCategories;
            string PublishedDate;
            dynamic volumeInfo;
            //

            livro.Id = array.id;
            volumeInfo = array.volumeInfo;

            livro.Title = volumeInfo.title;

            //if (volumeInfo.ContainsKey("subtitle"))
            //{
            //    livro.Subtitle = volumeInfo.subtitle;
            //}
            if (volumeInfo.ContainsKey("pageCount"))
            {
                livro.PageCount = volumeInfo.pageCount;
            }

            if (volumeInfo.ContainsKey("description"))
            {
                livro.Description = volumeInfo.description;
            }

            if (volumeInfo.ContainsKey("imageLinks"))
            {
                if (volumeInfo.imageLinks.ContainsKey("thumbnail"))
                {
                    livro.Thumbnail = volumeInfo.imageLinks.thumbnail;
                }
            }
            if (volumeInfo.ContainsKey("publisher"))
            {
                livro.Publisher = volumeInfo.publisher;
            }
            if (volumeInfo.ContainsKey("subtitle"))
            {
                livro.Subtitle = volumeInfo.subtitle;
            }
            if (volumeInfo.ContainsKey("publishedDate"))
            {
                try
                {
                    //tenta converter a data, que esteja no formato que é normalmente o padrão.
                    PublishedDate = string.Format("{0:dd/MM/yyyy}", (DateTime.ParseExact(((volumeInfo.publishedDate).ToString()), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)));
                }
                catch
                {
                    PublishedDate = volumeInfo.publishedDate;
                }
                if (!string.IsNullOrEmpty(livro.Publisher))
                {
                    livro.Publisher += " - " + PublishedDate;
                }
                else
                {
                    livro.Publisher = PublishedDate;
                }
              
            }

            if (volumeInfo.ContainsKey("authors"))
            {
                lstAuthors = volumeInfo.authors.ToObject<IList<string>>();
                strbdrarrays = new StringBuilder();
                foreach (string itnAuthors in lstAuthors)
                {
                    strbdrarrays.Append(itnAuthors);
                }
                livro.Authors = strbdrarrays.ToString();
            }

            if (volumeInfo.ContainsKey("categories"))
            {

                lstCategories = volumeInfo.categories.ToObject<IList<string>>();
                strbdrarrays = new StringBuilder();
                foreach (string itnCategories in lstCategories)
                {
                    strbdrarrays.AppendLine(itnCategories);
                }
                livro.Categories = strbdrarrays.ToString();
            }
            return livro;
        }

    }
}
