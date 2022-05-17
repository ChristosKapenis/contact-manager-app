using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for NewContactWindow.xaml
    /// </summary>
    public partial class NewContactWindow : Window
    {
        public NewContactWindow()
        {
            InitializeComponent();
        }

        private void AddContactBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = nameInput.Text;
            string cellphone = cellphoneInput.Text;
            string telephone = telephoneInput.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) return;

            SqliteDataAccess.SaveContact(new Contact { Name = name, CellphoneNumber = cellphone, TelephoneNumber = telephone });
            this.Close();
        }

        private void AddFromExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            openFileDialog.ShowDialog();

            if (!File.Exists(openFileDialog.FileName))
            {
                this.Close();
                return;
            }

            List<Contact> contacts = new List<Contact>();
            contacts = XLDataAccess.LoadExcel(openFileDialog.FileName);
            SqliteDataAccess.SaveContact(contacts);

            this.Close();
        }

     
    }
}