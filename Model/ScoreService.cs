using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Game_2048.Model
{
    public class ScoreService
    {
        private const string Url = @"http://localhost:48013/Score";

        private static readonly HttpClient client = new HttpClient();

        public static async Task<int> GetMaxAsync()
        {
            var response = await client.GetAsync(Url);
            var responseString = await response.Content.ReadAsStringAsync();

            return int.Parse(responseString);
        }

        public static void UpdateCurrent(Guid id, int result)
        {
            client.PostAsync(Url + "/" + id + "/" + result, null);
        }
    }
}