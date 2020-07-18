using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace HTTP_Client_Asp_Server
{
    class Program
    {
        public static async void Do()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/");

            HttpResponseMessage response = client.GetAsync("api/talkback/hello").Result;
            Console.WriteLine(response.ToString());
            var product = await response.Content.ReadAsStringAsync();
            Console.WriteLine(product);
        }

        public static async void And()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/");
           
            var array = new int[] { 1, 45, 1, 24, 5, 2 };

            var serial = JsonConvert.SerializeObject(array);
            //var encode = new enco(array);

            var content = new StringContent(serial, Encoding.UTF8, "application/json");
            //Console.WriteLine(content.ToString());

            //HttpResponseMessage response = await client.PostAsync("api/talkback/sort",content);
            var response = client.PostAsJsonAsync("api/talkback/sort", content).Result;

            Console.WriteLine(response.ToString());
            var product = await response.Content.ReadAsStringAsync();
            Console.WriteLine(product);
        }
        public static async void Builder()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/");
            var array = new int[] { 1, 45, 1, 24, 5, 2 };


            var builder = new UriBuilder("https://localhost:44324/");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["foo"] = "bar<>&-baz";
            query["bar"] = "bazinga";
            builder.Query = query.ToString();
            string url = builder.ToString();

            var serial = JsonConvert.SerializeObject(array);
            //var encode = new enco(array);

            var content = new StringContent(serial, Encoding.UTF8, "application/json");
            //Console.WriteLine(content.ToString());

            HttpResponseMessage response = await client.PostAsync("api/talkback/sort", content);

            Console.WriteLine(response.ToString());
            var product = await response.Content.ReadAsStringAsync();
            Console.WriteLine(product);
        }


        public static async void Add()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/");


            var prod = new Product() { Category = "asd", Name = "rand" };

            //HttpResponseMessage response = await client.PostAsync("api/talkback/sort",content);
            var response = await client.PostAsJsonAsync("api/products/add", prod);


            Console.WriteLine(response.ToString());
            var product = await response.Content.ReadAsStringAsync();
            Console.WriteLine(product);
        }

        static void Main()
        {
            ConsoleHandler ch = new ConsoleHandler();
            Do();
            And();
            Builder();

            while (true)
            {
                var line = Console.ReadLine();
                ch.ProcessLine(line);
            }
        }
    }
}
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}