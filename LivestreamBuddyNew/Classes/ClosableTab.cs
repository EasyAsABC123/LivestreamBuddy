using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveStreamBuddy.Controls;

namespace LiveStreamBuddy.Classes
{
	public class ClosableTab : TabItem
	{
		public delegate void ClosedHandler(object sender, EventArgs e);

		public delegate void ClosingHandler(object sender, EventArgs e);

		public ClosableTab()
		{
			var closableTabHeader = new CloseableHeader();

			Header = closableTabHeader;

			// Attach to the CloseableHeader events
			// (Mouse Enter/Leave, Button Click, and Label resize)
			closableTabHeader.button_close.MouseEnter +=
				button_close_MouseEnter;
			closableTabHeader.button_close.MouseLeave +=
				button_close_MouseLeave;
			closableTabHeader.button_close.Click +=
				button_close_Click;
			closableTabHeader.label_TabTitle.SizeChanged +=
				label_TabTitle_SizeChanged;
		}

		public string Title
		{
			set { ((CloseableHeader) Header).label_TabTitle.Content = value; }
		}

		protected override void OnSelected(RoutedEventArgs e)
		{
			base.OnSelected(e);
			((CloseableHeader) Header).button_close.Visibility = Visibility.Visible;
		}

		protected override void OnUnselected(RoutedEventArgs e)
		{
			base.OnUnselected(e);
			((CloseableHeader) Header).button_close.Visibility = Visibility.Hidden;
		}

		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			((CloseableHeader) Header).button_close.Visibility = Visibility.Visible;
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (!IsSelected)
			{
				((CloseableHeader) Header).button_close.Visibility = Visibility.Hidden;
			}
		}

		// Button MouseEnter - When the mouse is over the button - change color to Red
		private void button_close_MouseEnter(object sender, MouseEventArgs e)
		{
			((CloseableHeader) Header).button_close.Foreground = Brushes.Red;
		}

		// Button MouseLeave - When mouse is no longer over button - change color back to black
		private void button_close_MouseLeave(object sender, MouseEventArgs e)
		{
			((CloseableHeader) Header).button_close.Foreground = Brushes.Black;
		}

		// Button Close Click - Remove the Tab - (or raise
		// an event indicating a "CloseTab" event has occurred)
		private void button_close_Click(object sender, RoutedEventArgs e)
		{
			DoClosing();
			((TabControl) Parent).Items.Remove(this);
			DoClosed();
		}

		// Label SizeChanged - When the Size of the Label changes
		// (due to setting the Title) set position of button properly
		private void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((CloseableHeader) Header).button_close.Margin = new Thickness(
				((CloseableHeader) Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
		}

		public event ClosingHandler Closing;

		private void DoClosing()
		{
			if (Closing != null)
			{
				Closing(this, null);
			}
		}

		public event ClosedHandler Closed;

		private void DoClosed()
		{
			if (Closed != null)
			{
				Closed(this, null);
			}
		}
	}
}