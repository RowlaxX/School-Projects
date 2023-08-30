using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BDD.UI
{
    public partial class DoublePromptWindow : Window
    {
        public string? Result1 { get; private set; } = null;
        public string? Result2 { get; private set; } = null;

        public DoublePromptWindow(string question1, string question2)
        {
            InitializeComponent();
            Question1.Text = question1;
            Question2.Text = question2;

            Abort.Click += delegate { Close(); };
            Done.Click += delegate 
            { 
                Result1 = Answer1.Text.Length == 0 ? null : Answer1.Text; 
                Result2 = Answer2.Text.Length == 0 ? null : Answer2.Text; 
                Close(); 
            };
        }
    }
}
