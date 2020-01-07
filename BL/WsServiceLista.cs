using ML;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Net.Http;

namespace BL
{
    public class WsServiceLista
    {

        private int Total { get; set; }

        public async static Task<List<Book>> WsBusca(string busca, int key, string buscakey, int indice)
        {
            StringBuilder urlBusca = new StringBuilder();
            urlBusca.Append($"https://www.googleapis.com/books/v1/volumes?&startIndex={indice}&q=");
            //
            urlBusca.Append(busca);

            switch (key)
            {
                case 0:
                    break;
                case 1:
                    urlBusca.Append("+intitle:" + buscakey);
                    break;
                case 2:
                    urlBusca.Append("+inauthor:" + buscakey);
                    break;
                case 3:
                    urlBusca.Append("+subject:" + buscakey);
                    break;
                default:
                    break;
            }

            urlBusca.Append("&langRestrict=pt&key=AIzaSyAiqcLMIbWfBuTJCPA6AuEa-mUcPiZXS4E");
            try
            {
                using (var http = new HttpClient())
                {
                    HttpRequestMessage requisicao = new HttpRequestMessage
                    {
                        RequestUri = new Uri(urlBusca.ToString()),
                        Method = HttpMethod.Get
                    };
                    var resposta = await http.SendAsync(requisicao);
                    var conteudo = await resposta.Content.ReadAsStringAsync();
                  return JsonBusca(conteudo.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Book> JsonBusca(string json)
        {
            List<Book> Lista = new List<Book>();
            dynamic array = JsonConvert.DeserializeObject(json);
            if (array.totalItems > 0)
            {
                dynamic items = array.items;
                int itemscount = ((ICollection)array.items).Count;
                IList<string> lstAuthors;
                string PublishedDate;
                dynamic volumeInfo;
                //
                Book livro;

                for (int i = 0; i < itemscount; i++)
                {
                    //variaveis utilizadas na criação da lista
                    livro = new Book();

                    volumeInfo = items[i].volumeInfo;

                    livro.Id = items[i].id;

                    livro.Title = volumeInfo.title;

                    if (volumeInfo.ContainsKey("pageCount"))
                    {
                        livro.PageCount = volumeInfo.pageCount;
                    }
                    if (volumeInfo.ContainsKey("imageLinks"))
                    {
                        if (volumeInfo.imageLinks.ContainsKey("smallThumbnail"))
                        {
                            livro.Thumbnail = volumeInfo.imageLinks.smallThumbnail;
                        }
                    }

                    if (volumeInfo.ContainsKey("publisher"))
                    {
                        livro.Publisher = volumeInfo.publisher;
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
                        livro.Publisher += "-" + PublishedDate;
                    }

                    if (volumeInfo.ContainsKey("authors"))
                    {
                        lstAuthors = volumeInfo.authors.ToObject<IList<string>>();
                        string strbdrarrays = "";
                        foreach (string itnAuthors in lstAuthors)
                        {
                            if (string.IsNullOrEmpty(strbdrarrays.ToString()))
                            {
                                strbdrarrays = itnAuthors;
                            }
                            else
                            {
                                strbdrarrays += $"; {itnAuthors}";
                            }

                        }
                        livro.Authors = strbdrarrays;
                    }
                    Lista.Add(livro);
                }
             
            }
            return Lista;
        }

    }
}
