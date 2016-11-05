﻿using LipChat.Library;
using LipChat.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LipChat.Mobile.Helpers
{
    public class MessageService
    {
        private static readonly string ENVIRONMENT = "Production";

        public static async Task<IEnumerable<Message>> GetMessagesAsync(int last = 100)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.GetAPIAddress(ENVIRONMENT));

                var response = await client.GetAsync($"api/messages?last={last}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<IEnumerable<Message>>(content);

                    return messages;
                }

                return null;
            }
        }

        public static async Task<Message> PostMessageAsync(string content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.GetAPIAddress(ENVIRONMENT));

                var postMessage = JsonConvert.SerializeObject(new Message{ Content = content,
                    DateTime = DateTime.UtcNow.AddHours(-3), Sender = "Mobile" });
                var response = await client.PostAsync("api/messages", new StringContent(postMessage, Encoding.UTF8,
                    "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var requestContent = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<Message>(requestContent);

                    return message;
                }

                return null;
            }
        }
    }
}