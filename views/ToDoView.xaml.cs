using PersonalDashboard.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PersonalDashboard.Views
{
    public partial class TodoView : UserControl
    {
        private List<TaskItem> tasks = new();
        private string filePath = "tasks.json";

        public TodoView()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new();
            }

            TaskList.ItemsSource = null;
            TaskList.ItemsSource = tasks;
        }

        private void SaveTasks()
        {
            string json = JsonSerializer.Serialize(tasks);
            File.WriteAllText(filePath, json);
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string taskText = TaskInput.Text.Trim();
            if (!string.IsNullOrEmpty(taskText))
            {
                tasks.Add(new TaskItem { Title = taskText, IsDone = false });
                SaveTasks();
                LoadTasks();
                TaskInput.Text = "";
            }
        }

        private void TaskCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            SaveTasks();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var task = btn?.Tag as TaskItem;

            if (task != null)
            {
                tasks.Remove(task);
                SaveTasks();
                LoadTasks();
            }
        }
    }
}
