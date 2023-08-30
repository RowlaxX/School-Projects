using BDD.Core;
using System.Windows;

namespace BDD.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Database Database { get; } = new();

        private Context context;
        
        public Context Context
        {
            get
            {
                return context;
            }
            set
            {
                MainMenu.Children.Clear();
                MainMenu.Children.Add(value.Menu);
                MainContent.Children.Clear();
                MainContent.Children.Add(value.Content);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Context = new MainContext(this);
        }  
    }
}
