using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ModelLib.model;
using Newtonsoft.Json;

namespace ConsumingREST
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------- ItemConsumer - ready to run ----------");
            Console.WriteLine("Press a key to use Get ALL method...\n");
            Console.ReadKey();
            string itemWebApi = "https://itemsrvice.azurewebsites.net/api/items";

            GetAndPrintItems(itemWebApi);
            //PostNewItem();
            Console.ReadLine();
        }

        private static void GetAndPrintItems(string ItemWebApi)
        {
            Console.WriteLine("GET ALL ITEMS \n");
            List<Item> items = new List<Item>();
            try
            {
                Task<List<Item>> callTask = Task.Run(() => GetItems(ItemWebApi));
                callTask.Wait();
                items = callTask.Result;
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine(items[i].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task<List<Item>> GetItems(string ItemWebApi)
        {
            using (HttpClient client = new HttpClient())
            {
                string eventJsonString = await client.GetStringAsync(ItemWebApi);
                if (eventJsonString != null)
                    return (List<Item>) JsonConvert.DeserializeObject(eventJsonString, typeof(List<Item>));
                return null;
            }
        }
    }
}

