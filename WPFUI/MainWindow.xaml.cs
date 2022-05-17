using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Microsoft.Win32;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private List<Contact> contacts = new List<Contact>();

        public MainWindow()
        {
            InitializeComponent();

            InitialSetup();

            LoadContactsList();
        }

        private void InitialSetup()
        {
            editBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;

            contactsList.SelectionChanged += ContactsList_SelectionChanged;
        }

        private void LoadListViewItems(List<Contact> _contacts)
        {
            _contacts = _contacts.OrderBy(x => x.TelephoneNumber).ToList();

            contactsList.Items.Clear();
            foreach (var item in _contacts)
            {
                contactsList.Items.Add(item);
            }
        }

        /*
         * TODO:
         * Calling this function every time I update ONE contact and querying the whole database isn't very efficient.
         * I must redesign the way the list is updated, maybe use a buffer.
         */
        private void LoadContactsList()
        {
            contacts.Clear();
            contacts = SqliteDataAccess.LoadContacts();
            LoadListViewItems(contacts);
        }

        private void DeleteContact(Contact contact)
        {
            SqliteDataAccess.DeleteContact(contact);
            LoadContactsList();
        }

        private void Search(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                LoadListViewItems(contacts);
                return;
            }

            //TODO: Find a better way to do this
            LoadListViewItems(contacts.FindAll(x => x.Name.ToLower().Contains(input) || x.CellphoneNumber.Contains(input) || x.TelephoneNumber.Contains(input)).ToList());
        }

        private void ContactsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            contactsList.SelectedItem = null;
            editBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;

            Search(searchInput.Text);
            searchInput.Text = null;
        }

        private void SearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            editBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            Search(searchInput.Text);
        }

        private void NewContactBtn_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow window = new NewContactWindow();
            window.ShowDialog();
            LoadContactsList();
        }

        private void SaveToExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog;
            openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Contacts";
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            openFileDialog.ShowDialog();

            XLDataAccess.SaveXL(contacts, openFileDialog.FileName);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            EditContactWindow window = new EditContactWindow();

            window.id = (contactsList.SelectedItem as Contact).Id;
            window.name = window.nameInput.Text = (contactsList.SelectedItem as Contact).Name;
            window.cellphone = window.cellphoneInput.Text = (contactsList.SelectedItem as Contact).CellphoneNumber;
            window.telephone = window.telephoneInput.Text = (contactsList.SelectedItem as Contact).TelephoneNumber;
            
            window.ShowDialog();

            LoadContactsList();

            editBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (contactsList.SelectedItem == null)
            {
                throw new ArgumentNullException();
            }

            DeleteContact((Contact)contactsList.SelectedItem);

            contactsList.SelectedItem = null;
            editBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }
    }
}