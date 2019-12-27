﻿using BL.Models;
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

namespace BL
{
    public class WsService
    {

        List<Book> Lista = new List<Book>();
        private int Total { get; set; }


        public List<Book> WsBusca(string busca, int key, string buscakey, int indice)
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

            var requisicaoWeb = WebRequest.CreateHttp(urlBusca.ToString());
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                JsonBusca(objResponse.ToString());
                streamDados.Close();
                resposta.Close();
            }
            return Lista;
        }

        public void JsonBusca(string json)
        {
            Lista = new List<Book>();
            dynamic array = JsonConvert.DeserializeObject(json);

            //variaveis utilizadas na criação da lista
            Book livro = new Book();
            dynamic items = array.items;
            int itemscount = ((ICollection)array.items).Count;
            StringBuilder strbdrarrays = new StringBuilder();
            IList<string> lstAuthors;
            IList<string> lstCategories;
            string PublishedDate;
            dynamic volumeInfo;
            //

            for (int i = 0; i < itemscount; i++)
            {
                livro = new Book();

                livro.Id = items[i].Id;
                volumeInfo = items[i].volumeInfo;

                livro.Title = volumeInfo.title;
                
                //if (volumeInfo.ContainsKey("subtitle"))
                //{
                //    livro.Subtitle = volumeInfo.subtitle;
                //}
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
                Lista.Add(livro);
            }

            // var teste4 = teste["volumeInfo"]["title"];


        }

    }
}