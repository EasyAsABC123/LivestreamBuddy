using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Awesomium.Core;
using LiveStreamBuddy.Classes;
using LiveStreamBuddy.Controls;
using LobsterKnifeFight;
using Options = LiveStreamBuddy.Classes.Options;
using Stream = LiveStreamBuddy.Controls.Stream;

namespace LiveStreamBuddy
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			WebCore.Initialize(
				new WebConfig
				{
					PluginsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
				},
				true);

			visibleStreams = new List<string>();
			user = DataFileManager.GetUser();
			userOptions = DataFileManager.GetOptions();

			channelsControl.User = user;
			channelsControl.OnStreamOpen += channelsControl_OnStreamOpen;

			potentialNicknameColors = new[]
			{
				"#0000FF", "#FF0000", "#00FF00", "#9900CC", "#FF99CC",
				"#990000", "#3399FF", "#99CCFF", "#FF0033", "#33FF00",
				"#9933FF", "#FF3399", "#663300", "#669999", "#00FFFF",
				"#CC0000", "#FF9933", "#33F33", "#9999FF", "#CC0066",
				"#CC9966", "#FF6666", "#99FFCC", "#0033CC", "#666633"
			};

			streamTitleAutoCompleteOptions = new List<string>();
			streamTitleAutoCompleteOptions.AddRange(DataFileManager.GetStreamTitleAutoCompleteStrings());

			streamGameAutoCompleteOptions = new List<string>();
			streamGameAutoCompleteOptions.AddRange(DataFileManager.GetStreamGameAutoCompleteStrings());

			emoticons = null;

			string[] commandLineArgs = Environment.GetCommandLineArgs();

			bool refreshEmoticons =
				commandLineArgs.Any(arg => string.Compare(arg, "-refreshemoticons", StringComparison.OrdinalIgnoreCase) == 0);

			if (userOptions.ShowEmoticonsInChat)
			{
				try
				{
					emoticons = DataFileManager.GetEmoticons(refreshEmoticons);
				}
				catch
				{
				}
			}
		}

		# region Private Properties

		private readonly string[] potentialNicknameColors;
		private readonly List<string> streamGameAutoCompleteOptions;
		private readonly List<string> streamTitleAutoCompleteOptions;
		private readonly User user;
		private readonly List<string> visibleStreams;
		private List<Emoticon> emoticons;
		private Options userOptions;

		# endregion

		# region Private Methods

		private bool isStreamVisible(string channelName)
		{
			return visibleStreams.Any(stream => string.Compare(stream, channelName, StringComparison.OrdinalIgnoreCase) == 0);
		}

		# endregion

		# region Events

		private void channelsControl_OnStreamOpen(object sender, StreamOpenEventArgs e)
		{
			if (e != null && !string.IsNullOrEmpty(e.ChannelName) && !isStreamVisible(e.ChannelName))
			{
				Utility.GetAccessToken(user);

				try
				{
					if (!string.IsNullOrEmpty(user.AccessToken) && !string.IsNullOrEmpty(user.Name))
					{
						if (userOptions.ShowEmoticonsInChat && emoticons == null)
						{
							emoticons = DataFileManager.GetEmoticons();
						}

						visibleStreams.Add(e.ChannelName);

						if (userOptions.OpenStreamsInNewTab)
						{
							var stream = new Stream(user,
								e.ChannelName,
								user.AccessToken,
								potentialNicknameColors,
								streamTitleAutoCompleteOptions,
								streamGameAutoCompleteOptions,
								emoticons);

							var tab = new ClosableTab
							{
								Title = e.ChannelName,
								VerticalContentAlignment = VerticalAlignment.Stretch,
								Content = stream
							};

							tab.Closed += delegate
							{
								visibleStreams.Remove(e.ChannelName);
								stream.Disconnect();
							};

							mainTabs.Items.Add(tab);
						}
						else
						{
							var stream = new Stream(user,
								e.ChannelName,
								user.AccessToken,
								potentialNicknameColors,
								streamTitleAutoCompleteOptions,
								streamGameAutoCompleteOptions,
								emoticons);

							var ibd =
								new IconBitmapDecoder(new Uri("pack://application:,,,/LivestreamBuddyNew;component/livestream-ICON.ico"),
									BitmapCreateOptions.None, BitmapCacheOption.Default);
							var brush = new LinearGradientBrush((Color) ColorConverter.ConvertFromString("#FF515151"), Colors.LightGray,
								new Point(.5, 0), new Point(.5, 1));

							var newWindow = new Window
							{
								Width = 525,
								MinWidth = 525,
								Height = 675,
								MinHeight = 675,
								Title = e.ChannelName,
								Icon = ibd.Frames[0],
								Background = brush,
								Content = new Border {Padding = new Thickness(13, 13, 13, 13), Child = stream}
							};

							newWindow.Closed += delegate
							{
								visibleStreams.Remove(e.ChannelName);
								stream.Disconnect();
							};

							newWindow.Show();
						}
					}
					else
					{
						throw new Exception();
					}
				}
				catch
				{
					Utility.ClearUserData(user);
					MessageBox.Show("Something went wrong. Try again.");
				}
			}
		}

		private void HelpClick(object sender, RoutedEventArgs e)
		{
			var sInfo = new ProcessStartInfo("help.html");
			Process.Start(sInfo);
		}

		private void OptionsClick(object sender, RoutedEventArgs e)
		{
			var optionsControl = new Controls.Options();
			var ibd = new IconBitmapDecoder(new Uri("pack://application:,,,/LivestreamBuddyNew;component/livestream-ICON.ico"),
				BitmapCreateOptions.None, BitmapCacheOption.Default);
			var brush = new LinearGradientBrush((Color) ColorConverter.ConvertFromString("#FF515151"), Colors.LightGray,
				new Point(.5, 0), new Point(.5, 1));
			var newWindow = new Window
			{
				Width = 325,
				MinWidth = 325,
				Height = 375,
				MinHeight = 375,
				Title = "Options",
				Icon = ibd.Frames[0],
				Background = brush,
				Content = new Border {Padding = new Thickness(13, 13, 13, 13), Child = optionsControl}
			};

			optionsControl.OnSaved += delegate
			{
				userOptions = optionsControl.UserOptions;
				newWindow.Close();
			};

			newWindow.Show();
		}

		# endregion
	}
}