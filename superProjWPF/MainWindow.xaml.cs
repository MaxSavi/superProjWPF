using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace superProjWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
        //    InitializeComponent();
        //}
        private readonly HttpClient _httpClient;
        private ObservableCollection<Book> _books;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _books = new ObservableCollection<Book>();
            booksDataGrid.ItemsSource = _books;
            LoadBooks(); // Метод для загрузки книг из API
        }

        private async void LoadBooks()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7208/api/BookModels");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _books = JsonConvert.DeserializeObject<ObservableCollection<Book>>(responseBody);
                booksDataGrid.ItemsSource = _books;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке книг: {ex.Message}");
            }
        }

        private async void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book selectedBook = (sender as FrameworkElement).DataContext as Book;
                HttpResponseMessage response = await _httpClient.DeleteAsync($"https://localhost:7208/api/BookModels/{selectedBook.id}");
                ///{selectedBook.Id}
                response.EnsureSuccessStatusCode();
                _books.Remove(selectedBook);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при покупке книги: {ex.Message}");
            }
        }
    }

    public class Book
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public int yearOfPublication { get; set; }
    }
}
