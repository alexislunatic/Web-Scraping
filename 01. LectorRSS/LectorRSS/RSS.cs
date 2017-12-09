using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using ScrapySharp.Html.Forms;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace LectorRSS
{
    public class RSS
    {
        public List<string> fuentes;

        public RSS()
        {
            fuentes = new List<string>();
            fuentes.Add(@"https://elcomercio.pe/feed/politica");
            fuentes.Add(@"https://elcomercio.pe/feed/tecnologia");
            fuentes.Add(@"https://elcomercio.pe/feed/casa-y-mas");
            fuentes.Add(@"https://elcomercio.pe/feed/gastronomia");
            fuentes.Add(@"https://elcomercio.pe/feed/luces/cine");
            fuentes.Add(@"https://elcomercio.pe/feed/deporte-total");

        }

        public async Task<string> GetPage(string url)
        {

            var retStrin = "";

            var rawHTML = await DownloadWeb(url);
            var document = new HtmlDocument();

            document.LoadHtml(rawHTML);
            var nodes = document.DocumentNode.CssSelect(".parrafo.first-parrafo").ToList();
            StringBuilder strb = new StringBuilder();
            foreach (var node in nodes)
            {
                strb.AppendLine(node.InnerText.Trim().ToLower());
            }

            retStrin = strb.ToString();

            return retStrin;

        }

        public async Task CrearArchivo(string nombrearchivo, string texto)
        {
            using (StreamWriter streamWriter = new StreamWriter(nombrearchivo + ".txt", true, Encoding.UTF8))
            {
                streamWriter.WriteLine(texto);
            }
        }

        async Task<string> DownloadWeb(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(url, UriKind.Absolute);

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            if (response.Content != null)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                return result;
                            }
                        }
                    }
                }
            }
            return "";
        }
    }
}
