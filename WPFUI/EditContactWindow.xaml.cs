using System.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for EditContactWindow.xaml
    /// </summary>
    public partial class EditContactWindow : Window
    {
        public EditContactWindow()
        {
            InitializeComponent();
        }

        public int id;
        public string name;
        public string cellphone;
        public string telephone;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nameInput.Text == this.name && cellphoneInput.Text == this.cellphone && telephoneInput.Text == this.telephone)
            {
                this.Close();
                return;
            }

            SqliteDataAccess.DeleteContact(new Contact
            {
                Id = this.id
            });

            SqliteDataAccess.SaveContact(new Contact
            {
                Name = nameInput.Text,
                CellphoneNumber = cellphoneInput.Text,
                TelephoneNumber = telephoneInput.Text
            });

            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}