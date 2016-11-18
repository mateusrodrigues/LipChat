using LipChat.Library;
using LipChat.Library.Models;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LipChat.Monitor
{
    class Program
    {
        static IHubProxy _hub;
        static HubConnection _connection;
        static void Main(string[] args)
        {
            Console.WriteLine("Você deseja vizualizar as menssagens?(Y/N)");
            string resposta = Console.ReadLine();

            if(resposta == "y" || resposta == "Y")
            {
                 _connection = new HubConnection(Constants.GetAPIAddress("Production"));

                _hub = _connection.CreateHubProxy("ChatHub");

                _hub.On<string>("receive", message =>
                {
                    var newMessage = JsonConvert.DeserializeObject<Message>(message);
                    Console.WriteLine($"{newMessage.Sender} disse: {newMessage.Content}");
                });

                _connection.Start().Wait();
                Console.ReadLine();
            }
        }
    }
}
