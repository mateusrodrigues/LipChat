using LipChat.Library;
using LipChat.Library.Models;
using LipChat.Mobile.Helpers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LipChat.Mobile
{
    public partial class MainPage : ContentPage
    {
        HubConnection _connection;
        IHubProxy _hub;

        public ObservableCollection<Message> Messages { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Messages = new ObservableCollection<Message>();

            messagesListView.ItemsSource = Messages;

            _connection = new HubConnection(Constants.APIAddress);
            _hub = _connection.CreateHubProxy("ChatHub");
            Task.Factory.StartNew(() => _connection.Start().Wait());
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var messages = await MessageService.GetMessagesAsync();

            foreach (var m in messages.ToList())
            {
                Messages.Add(m);
            }

            Action<string> receiveMessage = (message) =>
            {
                var newMessage = new Message { MessageID = Guid.NewGuid(), Content = message };
                Device.BeginInvokeOnMainThread(() =>
                {
                    AddMessageToList(newMessage);
                });
            };

            _hub.On("receive", receiveMessage);
        }

        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            if (tbMessage.Text.Length > 0)
            {
                tbMessage.IsEnabled = false;
                await MessageService.PostMessageAsync(tbMessage.Text);
                await _hub.Invoke("Send", tbMessage.Text);
                tbMessage.IsEnabled = true;
                tbMessage.Text = string.Empty;
            }
        }

        #region Helpers
        private void AddMessageToList(Message message)
        {
            Messages.Add(message);
        }
        #endregion
    }
}
