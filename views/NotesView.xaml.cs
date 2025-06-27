using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PersonalDashboard.Models;

namespace PersonalDashboard.Views
{
    public partial class NotesView : UserControl
    {
        private List<Note> notes = new();
        private string filePath = "notes.json";

        public NotesView()
        {
            InitializeComponent();
            LoadNotes();
            RestoreTitlePlaceholder(null, null); // ensure placeholder shows on load
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new();
            }

            NotesList.ItemsSource = null;
            NotesList.ItemsSource = notes;
        }

        private void SaveNotesToFile()
        {
            string json = JsonSerializer.Serialize(notes);
            File.WriteAllText(filePath, json);
        }

        private void SaveNote(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text) || TitleBox.Text == "Title")
                return;

            var existing = NotesList.SelectedItem as Note;
            if (existing != null)
            {
                existing.Title = TitleBox.Text;
                existing.Content = ContentBox.Text;
            }
            else
            {
                notes.Add(new Note { Title = TitleBox.Text, Content = ContentBox.Text });
            }

            SaveNotesToFile();
            LoadNotes();
            ClearInputs();
        }

        private void DeleteNote(object sender, RoutedEventArgs e)
        {
            var selected = NotesList.SelectedItem as Note;
            if (selected != null)
            {
                notes.Remove(selected);
                SaveNotesToFile();
                LoadNotes();
                ClearInputs();
            }
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var note = NotesList.SelectedItem as Note;
            if (note != null)
            {
                TitleBox.Text = note.Title;
                TitleBox.Foreground = Brushes.Black;
                ContentBox.Text = note.Content;
            }
        }

        private void ClearInputs()
        {
            TitleBox.Text = "Title";
            TitleBox.Foreground = Brushes.Gray;
            ContentBox.Text = "";
            NotesList.SelectedItem = null;
        }

        private void ClearTitlePlaceholder(object sender, RoutedEventArgs e)
        {
            if (TitleBox.Text == "Title")
            {
                TitleBox.Text = "";
                TitleBox.Foreground = Brushes.Black;
            }
        }

        private void RestoreTitlePlaceholder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                TitleBox.Text = "Title";
                TitleBox.Foreground = Brushes.Gray;
            }
        }
    }
}
