using Confluent.Kafka;
using roomshare_room_service_query.Model;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace roomshare_room_service_query.Integration
{
    public static class AuthIntegration
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<User?> GetUserByGuid(Guid guid)
        {
            var result = await _client.GetAsync("http://host.docker.internal:5003/api/user/" + guid);
            var user = await result.Content.ReadAsStringAsync();

            if (user == null) return null;

            var userModel = JsonSerializer.Deserialize<User>(user);

            return userModel;
        }

        public static async Task<UserLogin?> GetUser(string token)
        {
            if(token == null) return null;

            token = token.Split(" ")[1];

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _client.GetAsync("http://host.docker.internal:5003/api/user");
            var user = await result.Content.ReadAsStringAsync();

            if (user == null) return null;

            var userModel = JsonSerializer.Deserialize<UserLogin>(user);

            return userModel;
        }
    }
}
