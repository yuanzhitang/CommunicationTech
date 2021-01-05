using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ChatRoom.Chat
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		HubConnection connection;
		string CurrentUser;

		public MainWindow()
		{
			InitializeComponent();

			connection = new HubConnectionBuilder()
				.WithUrl("https://localhost:44345/chatHub")
				.Build();

			connection.On<string>("Login", (msg) =>
			{
				MyCallBack_OnLogin(msg);
			});

			connection.On<string>("SignOut", (msg) =>
			 {
				 ChatCallback_OnLogout(msg);
			 });
			connection.On<string, string>("ReceiveMessage", (user, msg) =>
			{
				MainWindow_OnSayHello(user, msg);
			});

			connection.StartAsync();
		}

		private void Connect(object sender, RoutedEventArgs e)
		{
			var user = txtUser.Text.Trim();
			if (string.IsNullOrEmpty(user))
			{
				return;
			}

			CurrentUser = user;

			btnIn.IsEnabled = false;
			btnIn.Content = "Connecting";

			Task.Run(() =>
			{
				connection.InvokeAsync("Login", user);
				//btnSend.IsEnabled = true;


				//var chatCallback = new ChatCallBack(user);
				//chatCallback.OnSayHello += MainWindow_OnSayHello;
				//chatCallback.OnLogin += MyCallBack_OnLogin;
				//chatCallback.OnLogout += ChatCallback_OnLogout;

				//proxy = ProxyFactory.CreateChatServerProxy(chatCallback);
				//proxy.RegisterClient();

				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					btnSend.IsEnabled = true;
					btnIn.Content = "Connected";
				}));
			});
		}

		private void ChatCallback_OnLogout(string user)
		{
			Action method = new Action(() =>
			{
				if (txtInfo.Items.Contains(user))
				{
					txtInfo.Items.Remove(user);
				}

				txtMsg.Text += $"User:{user} is exited\r\n";
			});
			this.Dispatcher.BeginInvoke(method);
		}

		private void MyCallBack_OnLogin(string user)
		{
			Action method = new Action(() =>
						   {
							   txtInfo.Items.Add(user);

							   txtMsg.Text += $"User:{user} has joined the chat\r\n";

						   });
			this.Dispatcher.BeginInvoke(method);
		}

		private void MainWindow_OnSayHello(string user, string msg)
		{
			Action action = () =>
			{
				txtMsg.Text += $"{DateTime.Now.ToString()}\r\n[{user}]:{msg} \r\n\r\n";
			};

			Dispatcher.BeginInvoke(action);
		}

		private void btnOut_Click(object sender, RoutedEventArgs e)
		{
			connection.InvokeAsync("SignOut", CurrentUser);
			this.Close();
		}

		private void btnSend_Click(object sender, RoutedEventArgs e)
		{
			if (txtSend.Text == "")
			{
				return;
			}

			var message = txtSend.Text.Trim();
			Dispatcher.BeginInvoke(new Action(() =>
			{
				connection.InvokeAsync("SendMessage", CurrentUser, message);
			}));

			this.Dispatcher.Invoke(() =>
			{
				txtSend.Text = string.Empty;
			});
		}
	}
}
