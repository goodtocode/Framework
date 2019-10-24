using GoodToCode.Extensions;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GoodToCode.Framework.UserControls
{
    /// <summary>
    /// Copy clipboard glyph and functionality
    /// </summary>
    public sealed partial class ClipboardCopy : UserControl
    {
        /// <summary>
        /// Text to/from clipboard
        /// </summary>
        public string Text { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClipboardCopy()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Text copied to clipboard
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            SetClipboard(Text);
            PasteButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Text retrieved from clipboard
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private async void Paste_Click(object sender, RoutedEventArgs e)
        {
            Text = await GetClipboard();
        }

        /// <summary>
        /// Sets string text to clipboard
        /// </summary>
        /// <param name="text">String to set to clipboard</param>
        public void SetClipboard(string text)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }

        /// <summary>
        /// Sets string text to clipboard
        /// </summary>
        public async Task<string> GetClipboard()
        {
            var returnValue = Defaults.String;
            var dataPackageView = Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.Text))
                returnValue = await dataPackageView.GetTextAsync();                

            return returnValue;
        }
    }
}
