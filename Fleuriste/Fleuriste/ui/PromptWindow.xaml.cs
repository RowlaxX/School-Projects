using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class PromptWindow : Window
    {
        public string? Result { get; private set; } = null;

        public PromptWindow(string question)
        {
            InitializeComponent();
            Question.Text = question;

            Abort.Click += delegate { Close(); };
            Done.Click += delegate { Result = Answer.Text.Length == 0 ? null : Answer.Text; Close(); };
        }
    }
}
