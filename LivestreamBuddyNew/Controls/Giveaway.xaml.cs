using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace LiveStreamBuddy.Controls
{
	/// <summary>
	///     Interaction logic for Giveaway.xaml
	/// </summary>
	public partial class Giveaway : UserControl
	{
		public Giveaway()
		{
			InitializeComponent();
		}

		public Giveaway(List<string> viewers, ref List<string> excludeList)
			: this()
		{
			Viewers = viewers;
			ExcludeList = excludeList;

			foreach (string exclude in ExcludeList)
			{
				txtExclude.Text += exclude + Environment.NewLine;
			}
		}

		# region Private Members

		private string winner;

		# endregion

		# region Public Members

		public List<string> Viewers { get; set; }

		public List<string> ExcludeList { get; set; }

		# endregion

		# region Private Methods

		private void populateExcludeList()
		{
			ExcludeList.Clear();

			string[] excludeList = txtExclude.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

			foreach (string exclude in excludeList)
			{
				ExcludeList.Add(exclude);
			}
		}

		# endregion

		# region Public Methods

		public void Finalize()
		{
			if (!string.IsNullOrEmpty(winner))
			{
				txtExclude.Text += winner + Environment.NewLine;
			}

			populateExcludeList();
		}

		# endregion

		# region Events

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (Viewers.Count > 0)
			{
				populateExcludeList();

				foreach (string exclude in ExcludeList)
				{
					Viewers.Remove(exclude);
				}

				var random = new Random();
				int randomNumber = random.Next(0, Viewers.Count);

				winner = Viewers[randomNumber];

				var dispatcherTimer = new DispatcherTimer();

				int i = 0;
				string[] words = {"AND ", "THE ", "WINNER ", "IS ", ".", ".", "."};

				dispatcherTimer.Tick += delegate
				{
					if (i < words.Length)
					{
						lblStatus.Text += words[i];
						i++;

						CommandManager.InvalidateRequerySuggested();
					}
					else
					{
						lblWinner.Text = winner;
					}
				};

				dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 750);
				dispatcherTimer.Start();
			}
		}

		# endregion
	}
}