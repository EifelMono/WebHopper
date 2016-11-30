using Nancy;
using Nancy.Hosting.Self;
using System;

namespace WebHopper
{

    public class Module : NancyModule
    {
        string Port = "4711";
        string HtmlRedirect =
              "<html>"
            + "  <head>"
            + "    <meta http-equiv=\"refresh\" content=\"2; URL={1}/Start\">"
            + "  </head>"
            + "  <body>"
            + "    <h1>Diese Seite {0}/Start existiert nicht, Sie werden weitergeleitet.</h1>"
            + "    <p>Falls Ihr Browser keine automatische Weiterleitung unterstützt, <a href = \"{1}/Start\" > klicken Sie hier</a>!"
            + "  </body>"
            + "</html>";
        string Html =
              "<html>"
            + "  <body>"
            + "    <h1>Die Seite {0}/Start wurde gefunden.</h1>"
            + "  </body>"
            + "</html>";
        public Module() : base()
        {
            Get["/Start"] = parameters =>
            {
                if (string.IsNullOrEmpty(Program.PortRedirect))
                    return string.Format(Html, Program.Url);
                else
                    return string.Format(HtmlRedirect, Program.Url, Program.UrlRedirect);
            };
        }
    }
    public class Program
    {
        public static string Port = "4711";
        public static string PortRedirect = "4712";
        public static string Url = CreateUrl(Port);
        public static string UrlRedirect = CreateUrl(PortRedirect);


        public static string CreateUrl(string port)
        {
            return string.Format($"http://localhost:{port}");
        }
        static void Main(string[] args)
        {
            try
            {
                Port = Environment.GetCommandLineArgs()[1];
                PortRedirect = "";
                if (Environment.GetCommandLineArgs().Length > 2)
                    PortRedirect = Environment.GetCommandLineArgs()[2];

                Url = CreateUrl(Port);
                UrlRedirect = CreateUrl(PortRedirect);
                Console.Title = Url;
                if (!string.IsNullOrEmpty(PortRedirect))
                    Console.Title = $"{Url} -> {UrlRedirect}";

                Console.WriteLine("Server started !");
                using (NancyHost host = new NancyHost(new Uri(Url)))
                {

                    host.Start();
                    while (true)
                    {
                        if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
                            break;
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
