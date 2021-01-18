using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml.Shapes;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StandardWorks.BookOfMormon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Title : Page
    {
        public Title()
        {
            this.InitializeComponent();
            Open();
        }

        private async void Open()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/BookOfMormon/Title.pdf"));

            // Open and display the PDF.
            await OpenPDFAsync(file);
            await DisplayPage(0);
        }

        private PdfDocument _myDocument { get; set; }

        private async Task OpenPDFAsync(StorageFile file)
        {
            if (file == null) throw new ArgumentNullException();

            _myDocument = await PdfDocument.LoadFromFileAsync(file);
        }

        private async Task DisplayPage(uint pageIndex)
        {
            if (_myDocument == null)
            {
                throw new Exception("No document open.");
            }

            if (pageIndex >= _myDocument.PageCount)
            {
                throw new ArgumentOutOfRangeException($"Document has only {_myDocument.PageCount} pages.");
            }

            // Get the page you want to render.
            var page = _myDocument.GetPage(pageIndex);

            // Create an image to render into.
            var image = new BitmapImage();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await page.RenderToStreamAsync(stream);
                await image.SetSourceAsync(stream);

                // Set the XAML `Image` control to display the rendered image.
                PdfImage.Source = image;
            }
        }

        private void InkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            InkCanvas canvas = sender as InkCanvas;

            //Set inputs
            canvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;

            // Set initial ink stroke attributes.
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Size = new Size(10, 10);
            drawingAttributes.Color = Windows.UI.Colors.Black;
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            canvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }
    }
}
