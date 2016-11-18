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
        // Responsible for storing connection information
        // on the real-time server
        static IHubProxy _hub;
        static HubConnection _connection;

        static void Main(string[] args)
        {
            // "Would you like to see the messages?" prompt
            Console.WriteLine("Você deseja vizualizar as menssagens?(Y/N)");
            string answer = Console.ReadLine();

            // If yes, then start listening for new messages
            if (answer == "y" || answer == "Y")
            {
                // Open connection to the real-time endpoint address
                 _connection = new HubConnection(Constants.GetAPIAddress("Production"));
                // Create a hub proxy to the ChatHub
                _hub = _connection.CreateHubProxy("ChatHub");
                // Describe the block of code to run in case of message received
                _hub.On<string>("receive", message =>
                {
                    var newMessage = JsonConvert.DeserializeObject<Message>(message);
                    Console.WriteLine($"{newMessage.Sender} disse: {newMessage.Content}");
                });
                // Start the connection
                _connection.Start().Wait();
                Console.ReadLine();
            }
        }
    }
}
