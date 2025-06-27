using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PersonalDashboard.Models;

namespace PersonalDashboard.Views
{
    public partial class EventsView : UserControl
    {
        private List<EventItem> events = new();
        private string filePath = "events.json";

        public EventsView()
        {
            InitializeComponent();
            LoadEvents();
            RestoreTitlePlaceholder(null, null); // apply placeholder on load
        }

        private void LoadEvents()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                events = JsonSerializer.Deserialize<List<EventItem>>(json) ?? new();
            }

            events = events.OrderBy(e => e.Date).ToList();
            EventList.ItemsSource = null;
            EventList.ItemsSource = events;
        }

        private void SaveEvents()
        {
            string json = JsonSerializer.Serialize(events);
            File.WriteAllText(filePath, json);
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            string title = EventTitleBox.Text.Trim();
            DateTime? date = EventDatePicker.SelectedDate;

            if (!string.IsNullOrEmpty(title) && title != "Event Title" && date != null)
            {
                events.Add(new EventItem { Title = title, Date = date.Value });
                SaveEvents();
                LoadEvents();
                EventTitleBox.Text = "Event Title";
                EventTitleBox.Foreground = Brushes.Gray;
                EventDatePicker.SelectedDate = DateTime.Now;
            }
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var evt = button?.Tag as EventItem;

            if (evt != null)
            {
                events.Remove(evt);
                SaveEvents();
                LoadEvents();
            }
        }

        private void ClearTitlePlaceholder(object sender, RoutedEventArgs e)
        {
            if (EventTitleBox.Text == "Event Title")
            {
                EventTitleBox.Text = "";
                EventTitleBox.Foreground = Brushes.Black;
            }
        }

        private void RestoreTitlePlaceholder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EventTitleBox.Text))
            {
                EventTitleBox.Text = "Event Title";
                EventTitleBox.Foreground = Brushes.Gray;
            }
        }
    }
}
