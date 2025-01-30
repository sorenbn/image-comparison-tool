using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageComparisonTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        UpdateUI();
    }

    private void Window_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
        else
            e.Effects = DragDropEffects.None;
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                AddFileButton(file);
            }

            UpdateUI();
        }
    }

    private void AddFileButton(string filePath)
    {
        var fileButton = new Button
        {
            Content = System.IO.Path.GetFileName(filePath),
            Margin = new Thickness(5),
            Tag = filePath // Store file path inside the button
        };

        fileButton.MouseEnter += FileButton_Hover;
        fileButton.Click += FileButton_Click;

        ButtonPanel.Children.Add(fileButton);

        if (ButtonPanel.Children.Count == 1)
            ShowImage(filePath);

        UpdateUI();
    }

    private void FileButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string filePath)
        {
            RemoveImageButton(filePath);
        }
    }

    private void FileButton_Hover(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string filePath)
        {
            ShowImage(filePath);
        }
    }

    private void ShowImage(string filePath)
    {
        try
        {
            MainImage.Source = new BitmapImage(new Uri(filePath));
            MainImage.Visibility = Visibility.Visible;
            PlaceholderText.Visibility = Visibility.Collapsed;
        }
        catch
        {
            MessageBox.Show("Unable to load image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RemoveImageButton(string filePath)
    {
        var button = ButtonPanel
            .Children
            .OfType<Button>()
            .FirstOrDefault(x => x.Tag is string tagStr && tagStr == filePath);

        if (button != null)
        {
            ButtonPanel.Children.Remove(button);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (ButtonPanel.Children.Count == 0)
        {
            PlaceholderText.Visibility = Visibility.Visible;
            MainImage.Visibility = Visibility.Collapsed;
        }
        else if (MainImage.Source == null)
        {
            MainImage.Visibility = Visibility.Collapsed;
        }
    }
}
