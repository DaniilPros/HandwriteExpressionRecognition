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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Tesseract;

namespace HandwriteExpressionRecognition.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var showDialog = fileDialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                //@"D:\HandwriteExpressionRecognition\HandwriteExpressionRecognition\HandwriteExpressionRecognition.Desktop\TestData\123456789.bmp"
                var tessEngine = new Tesseract.TesseractEngine("tessdata", "eng", EngineMode.Default);
                var page = tessEngine.Process(
                    Pix.LoadFromFile(fileDialog.FileName),
                    PageSegMode.Auto);
                TextBlock.Text = page.GetText();
            }
        }
    }
}
