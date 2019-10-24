

using Windows.UI.Xaml;
using GoodToCode.Framework.Pages;

namespace GoodToCode.Framework.UserControls
{
    /// <summary>
    /// Interaction logic for HamburgerMenu.xaml
    /// </summary>
    public class HamburgerMenu : ReadOnlyControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HamburgerMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <param name="modelData">Data to bind to page</param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Partial and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Partial_Loaded(object sender, RoutedEventArgs e)
        {
            base.Partial_Loaded(sender, e);
        }        

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MyApplication.Navigate(MyApplication.HomePage);
        }

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MyApplication.Navigate(MyApplication.HomePage);
        }

        /// <summary>
        /// Navigates to CustomerCreate screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            MyApplication.RootFrame.Navigate(MyApplication.HomePage);
        }        
    }
}