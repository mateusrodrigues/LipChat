using LipChat.Library;
using LipChat.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LipChat.Universal.Helpers
{
    public static class MessageService
    {
        private static readonly int TIMEZONE = -3;
        private static readonly string ENVIRONMENT = "Production";

        /// <summary>
        /// Request a # of messages persisted in the API.
        /// </summary>
        /// <param name="last"># of last messages to return</param>
        /// <returns>A list of the messages persisted in the API</returns>
        public static async Task<IEnumerable<Message>> GetMessagesAsync(int last = 100)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.GetAPIAddress(ENVIRONMENT));
                // Make GET request to the API to retrieve messages
                var response = await client.GetAsync($"api/messages?last={last}");

                if (response.IsSuccessStatusCode)
                {
                    // Read the content body from the response
                    var content = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON into a list of Message objects
                    var messages = JsonConvert.DeserializeObject<IEnumerable<Message>>(content);

                    return messages;
                }

                return null;
            }
        }

        /// <summary>
        /// Send a new message to be persisted in the API
        /// </summary>
        /// <param name="content">The content of the message</param>
        /// <returns>The newly created Message object returned from the 201 Content Created HTTP response</returns>
        public static async Task<Message> PostMessageAsync(string content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.GetAPIAddress(ENVIRONMENT));
                // Serialize into JSON the new Message object to be sent to the API
                var postMessage = JsonConvert.SerializeObject(new Message { Content = content,
                    DateTime = DateTime.UtcNow.AddHours(TIMEZONE), Sender = "Universal" });
                // POST the serialized data to the API
                var response = await client.PostAsync("api/messages", new StringContent(postMessage, Encoding.UTF8,
                    "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    // Read the content body from the response
                    var requestContent = await response.Content.ReadAsStringAsync();
                    // Deserialize the new created object's JSON into a list of Messages
                    var message = JsonConvert.DeserializeObject<Message>(requestContent);

                    return message;
                }

                return null;
            }
        }
    }
}
