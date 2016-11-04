using LipChat.Library;
using LipChat.Library.Models;
using LipChat.Universal.Helpers;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LipChat.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HubConnection connection;
        IHubProxy _hub;
        public ObservableCollection<Message> Messages { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Messages = new ObservableCollection<Message>();

            messagesListView.ItemsSource = Messages;

            connection = new HubConnection(Constants.APIAddress);
            _hub = connection.CreateHubProxy("ChatHub");
            Task.Factory.StartNew(() => connection.Start().Wait());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var messages = await MessageService.GetMessagesAsync();
            messages.ToList().ForEach(m =>
            {
                Messages.Add(m);
            });

            _hub.On("receive", async (message) =>
            {
                var newMessage = new Message { MessageID = Guid.NewGuid(), Content = message };
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        AddMessageToList(newMessage);
                    });
            });
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
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
