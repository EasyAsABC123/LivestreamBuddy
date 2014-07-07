﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using LiveStreamBuddy.Classes;
using LobsterKnifeFight;

namespace LiveStreamBuddy.Controls
{
	/// <summary>
	///     Interaction logic for Streams.xaml
	/// </summary>
	public partial class Channels : UserControl
	{
		public Channels()
		{
			InitializeComponent();
			Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;

			channels = new ObservableCollection<ChannelInfo>();
			onlineImage = new BitmapImage(new Uri("pack://application:,,,/LivestreamBuddyNew;component/Resources/check.png"));
			offlineImage = new BitmapImage(new Uri("pack://application:,,,/LivestreamBuddyNew;component/Resources/grayX.png"));

			addStreamToFavoritesList("teamxim");

			foreach (string channel in DataFileManager.GetFavoriteChannels())
			{
				addStreamToFavoritesList(channel, true);
			}

			streamManager = new StreamManager();

			grdChannels.ItemsSource = channels;
			firstReportDone = false;

			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += worker_DoWork;
			worker.ProgressChanged += worker_ProgressChanged;
			worker.RunWorkerAsync();
		}

		public Channels(User user)
			: this()
		{
			User = user;
		}

		# region Public Properties

		public delegate void StreamOpenHandler(object sender, StreamOpenEventArgs e);

		public User User { get; set; }

		public event StreamOpenHandler OnStreamOpen;

		# endregion

		# region Private Properties

		private readonly ObservableCollection<ChannelInfo> channels;
		private readonly BitmapImage offlineImage;
		private readonly BitmapImage onlineImage;
		private readonly StreamManager streamManager;
		private readonly BackgroundWorker worker;
		private bool firstReportDone;

		# endregion

		# region Private Methods

		private void openStream(string channelName)
		{
			if (OnStreamOpen != null)
			{
				var args = new StreamOpenEventArgs {ChannelName = channelName.Trim()};
				OnStreamOpen(this, args);
			}
		}

		private bool streamExists(string channelName)
		{
			return
				channels.Any(channelInfo => string.Compare(channelInfo.Name, channelName, StringComparison.OrdinalIgnoreCase) == 0);
		}

		private void addStreamToFavoritesList(string streamName, bool isFavorite = false)
		{
			if (!streamExists(streamName))
			{
				channels.Add(new ChannelInfo {Name = streamName, IsFavoriteChannel = isFavorite});
			}
		}

		private void addStreamToFavoritesList(LobsterKnifeFight.Stream stream, bool isFavorite = false)
		{
			if (stream.Channel != null && !streamExists(stream.Channel.Name))
			{
				BitmapImage indicator = offlineImage;

				if (stream.IsOnline)
				{
					indicator = onlineImage;
				}

				channels.Add(new ChannelInfo
				{
					Name = stream.Channel.Name,
					StreamTitle = stream.Channel.Title,
					Game = stream.Game,
					Viewers = stream.ViewerCount,
					OnlineIndicator = indicator,
					IsFavoriteChannel = isFavorite
				});
			}
		}

		private string getChannelsFromList()
		{
			return channels.Aggregate(string.Empty, (current, channel) => current + (channel.Name + ","));
		}

		private void workerCheckChannelsStatus()
		{
			string channels = getChannelsFromList();
			List<LobsterKnifeFight.Stream> streams = null;

			if (!string.IsNullOrEmpty(channels))
			{
				streams = streamManager.GetStreams(channels);

				if (streams != null)
				{
					worker.ReportProgress(0, streams);
				}
			}
		}

		# endregion

		# region Events

		private void btnAddToFavoriteChannel_Click(object sender, RoutedEventArgs e)
		{
			var window = new SingleTextBoxWindow("Add Favorite Channel", "Channel: ", "Add");

			window.Owner = Application.Current.MainWindow;

			if ((bool) window.ShowDialog())
			{
				DataFileManager.AddFavoriteChannel(window.Value);
				LobsterKnifeFight.Stream stream = streamManager.GetStream(window.Value.ToLower());

				if (stream.IsOnline)
				{
					addStreamToFavoritesList(stream, true);
				}
				else
				{
					addStreamToFavoritesList(window.Value.ToLower(), true);
				}
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			workerCheckChannelsStatus();

			while (!worker.CancellationPending)
			{
				Thread.Sleep(60000);

				workerCheckChannelsStatus();
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			var streams = e.UserState as List<LobsterKnifeFight.Stream>;

			foreach (ChannelInfo channel in channels)
			{
				bool found = false;

				foreach (
					LobsterKnifeFight.Stream stream in
						streams.Where(stream => string.Compare(channel.Name, stream.Channel.Name, StringComparison.OrdinalIgnoreCase) == 0)
					)
				{
					channel.StreamTitle = stream.Channel.Title;
					channel.Game = stream.Game;
					channel.Viewers = stream.ViewerCount;
					channel.OnlineIndicator = onlineImage;

					found = true;
					break;
				}

				if (!found)
				{
					channel.StreamTitle = string.Empty;
					channel.Game = string.Empty;
					channel.Viewers = 0;
					channel.OnlineIndicator = offlineImage;
				}
			}

			if (!firstReportDone)
			{
				btnShowFeaturedStreams.IsEnabled = true;
				btnShowFollowedStreams.IsEnabled = true;
				btnAddToFavoriteChannel.IsEnabled = true;
				btnGoToChannel.IsEnabled = true;
				firstReportDone = true;
			}
		}

		private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
		{
			worker.CancelAsync();
		}

		private void btnGoToChannel_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtGoToChannel.Text))
			{
				MessageBox.Show("You must provide a channel name.");
			}
			else
			{
				openStream(txtGoToChannel.Text.ToLower());
			}
		}

		private void grdChannels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ChannelInfo channel = null;

			try
			{
				if (grdChannels.SelectedItems.Count > 0)
				{
					channel = (ChannelInfo) grdChannels.SelectedItems[0];
				}
			}
			catch
			{
			}

			if (channel != null)
			{
				openStream(channel.Name);
			}
		}

		private void ShowFeaturedStreamsClick(object sender, RoutedEventArgs e)
		{
			foreach (LobsterKnifeFight.Stream stream in streamManager.GetFeaturedStreams())
			{
				addStreamToFavoritesList(stream);
			}
		}

		private void ShowFollowedStreamsClick(object sender, RoutedEventArgs e)
		{
			Utility.GetAccessToken(User);

			try
			{
				if (!string.IsNullOrEmpty(User.AccessToken))
				{
					foreach (LobsterKnifeFight.Stream stream in streamManager.GetFollowedStreams(User))
					{
						addStreamToFavoritesList(stream);
					}
				}
				else
				{
					throw new Exception();
				}
			}
			catch
			{
				Utility.ClearUserData(User);

				MessageBox.Show("Something went wrong. Try again.");
			}
		}

		private void RemoveChannelClick(object sender, RoutedEventArgs e)
		{
			if (grdChannels.SelectedItems.Count > 0)
			{
				MessageBoxResult result = MessageBox.Show("Are you sure you want to remove the selected channel(s)?", "Confirm",
					MessageBoxButton.YesNo);

				if (result == MessageBoxResult.Yes)
				{
					var channelsToRemove = new List<ChannelInfo>();

					foreach (ChannelInfo channelInfo in grdChannels.SelectedItems)
					{
						if (string.Compare("teamxim", channelInfo.Name, StringComparison.OrdinalIgnoreCase) != 0)
						{
							if (channelInfo.IsFavoriteChannel)
							{
								DataFileManager.RemoveFavoriteChannel(channelInfo.Name);
							}

							channelsToRemove.Add(channelInfo);
						}
					}

					foreach (ChannelInfo channelInfo in channelsToRemove)
					{
						channels.Remove(channelInfo);
					}
				}
			}
		}

		private void grdChannels_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (grdChannels.SelectedIndex > -1)
			{
				btnRemoveChannel.IsEnabled = true;
			}
			else
			{
				btnRemoveChannel.IsEnabled = false;
			}
		}

		# endregion
	}

	public class StreamOpenEventArgs : EventArgs
	{
		public string ChannelName { get; set; }
	}

	public class ChannelInfo : INotifyPropertyChanged
	{
		private string game = string.Empty;
		private string name = string.Empty;
		private BitmapImage onlineIndicator;

		private string streamTitle = string.Empty;
		private long viewers;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				RaisePropertyChanged("Name");
			}
		}

		public string StreamTitle
		{
			get { return streamTitle; }
			set
			{
				streamTitle = value;
				RaisePropertyChanged("StreamTitle");
			}
		}

		public string Game
		{
			get { return game; }
			set
			{
				game = HttpUtility.HtmlDecode(value);
				RaisePropertyChanged("Game");
			}
		}

		public long Viewers
		{
			get { return viewers; }
			set
			{
				viewers = value;
				RaisePropertyChanged("Viewers");
			}
		}

		public BitmapImage OnlineIndicator
		{
			get { return onlineIndicator; }
			set
			{
				onlineIndicator = value;
				RaisePropertyChanged("OnlineIndicator");
			}
		}

		public bool IsFavoriteChannel { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string caller)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}
	}
}