using System;
using System.Linq;
using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace LectorRSS
{
    class MainClass
    {
        public static void Main(string[] args)
        {
                Task.Run(async () =>
                {

                    var rssobj = new RSS();
                    var listafuentes = rssobj.fuentes;



                    foreach (var fuente in listafuentes)
                    {
                        var uri = new Uri(fuente);
                        var categoria = uri.Segments.Last();
                        try
                        {
                            var feeds = await FeedReader.ReadAsync(fuente);

                            foreach (var feed in feeds.Items)
                            {
                                var texto = await rssobj.GetPage(feed.Link);
                                await rssobj.CrearArchivo(categoria, texto);
                            }
                        }
                        catch (Exception)
                        {


                        }



                    }

                }).GetAwaiter().GetResult();
        }
    }
}
