using Calculator.Models;
using System.Collections.Generic;
using System.Windows;

namespace Calculator.Views
{
    public partial class MemoryHistoryWindow : Window
    {
        public MemoryHistoryItem SelectedMemoryItem { get; private set; }
        public bool UseSelectedValue { get; private set; }

        public MemoryHistoryWindow(List<MemoryHistoryItem> memoryItems)
        {
            InitializeComponent();
            MemoryListView.ItemsSource = memoryItems;
            UseSelectedValue = false;
        }

        private void UseSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedMemoryItem = MemoryListView.SelectedItem as MemoryHistoryItem;
            if (SelectedMemoryItem != null)
            {
                UseSelectedValue = true;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a memory item first", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}