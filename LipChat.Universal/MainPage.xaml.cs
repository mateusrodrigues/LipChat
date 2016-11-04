using LipChat.Library.Models;
using LipChat.Universal.Helpers;
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
        public ObservableCollection<Message> Messages { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Messages = new ObservableCollection<Message>();

            messagesListView.ItemsSource = Messages;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var messages = await MessageService.GetMessagesAsync();
            messages.ToList().ForEach(m =>
            {
                Messages.Add(m);
            });
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (tbMessage.Text.Length > 0)
            {
                tbMessage.IsEnabled = false;
                await MessageService.PostMessageAsync(tbMessage.Text);
                tbMessage.IsEnabled = true;
                tbMessage.Text = string.Empty;
            }
        }
    }
}
