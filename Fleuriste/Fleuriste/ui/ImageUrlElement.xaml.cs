using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD.UI
{
    public partial class ImageUrlElement : UserControl
    {
        private static readonly BitmapImage WAIT = new(new Uri("/ui/ImageUrlElementWait.png", UriKind.Relative));
        private static readonly BitmapImage ERROR = new(new Uri("/ui/ImageUrlElementError.png", UriKind.Relative));

        public ImageUrlElement()
        {
            InitializeComponent();
        }

        public void SetURL(string value)
        {
            Task.Factory.StartNew(() =>
            {
                Apply(WAIT);
                BitmapImage bmp = Load(value);
                Apply(bmp);
            });
        }

        private void Apply(BitmapImage bmp)
        {
            Dispatcher.Invoke(() => IMG.Source = bmp);
        }

        public static BitmapImage Load(string value)
        {
            Task<BitmapImage> t = LoadAsync(value);
            t.Wait();
            return t.Result;
        }

        public static async Task<BitmapImage> LoadAsync(string url)
        {
            var httpClient = new HttpClient();

            try
            {
                using var response = await httpClient.GetAsync(new Uri(url, UriKind.Absolute));

                if (response.IsSuccessStatusCode)
                {
                    using var stream = new MemoryStream();
                    await response.Content.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    BitmapImage bitmap = new();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch(Exception) {}

            return ERROR;
        }
    }

}
