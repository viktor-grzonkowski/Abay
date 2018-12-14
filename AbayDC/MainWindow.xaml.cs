using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System;
using DedicatedCliend.ItemServiceReference;
using System.Collections.Generic;

namespace DedicatedClient
{
    public partial class MainWindow : Window
    {
        private ItemServiceClient itemService;
        private ObservableCollection<Item> items;
        private Item selectedItem;
        private DedicatedCliend.UserServiceReference.User user;

        public MainWindow(DedicatedCliend.UserServiceReference.User user)
        {
            InitializeComponent();

            itemService = new ItemServiceClient("NetTcpBinding_IItemService");

            this.user = user;

            //Fill up DataContext for DataGrid
            items = new ObservableCollection<Item>(itemService.GetAllItems());
            dgItems.DataContext = items;

            selectedItem = null;
        }

        private void UpdateDataGrid()
        {
            items = new ObservableCollection<Item>(itemService.GetAllItems());
            dgItems.DataContext = items;
        }
        private void FillFormWithItem(Item item)
        {
            lblId.Content = item.Id;
            lblCategory.Content = item.Category.Name;
            txtName.Text = item.Name;
            txtDescribe.Text = item.Description;
            lblStartDate.Content = item.StartDate;
            lblEndDate.Content = item.EndDate;
            lblSeller.Content = item.SellerUser.UserName;
            chkSold.IsChecked = item.State > 0;

            List<Bid> bids = new List<Bid>();
            bids.Add(item.WinningBid);
            foreach (Bid bid in item.OldBids)
            {

            }
            lvBids.DataContext = bids;
        }
        private void ResetForm()
        {
            lblId.Content = "-";
            lblCategory.Content = "-";
            txtName.Text = "-";
            txtDescribe.Text = "-";
            lblStartDate.Content = "-";
            lblEndDate.Content = "-";
            lblSeller.Content = "-";
            chkSold.IsChecked = false;
        }
        private bool IsItemSelected()
        {
            return selectedItem != null;
        }

        private void dgItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = (Item)dgItems.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }

            FillFormWithItem(selectedItem);
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsItemSelected())
            {
                return;
            }

            bool success = itemService.UpdateItem(selectedItem.Id, user.LoginToken.SecureToken, selectedItem.Name,
                selectedItem.Description, selectedItem.Category.Id);
            if (!success)
            {
                MessageBox.Show("Something went wrong! Try again!");
                return;
            }

            UpdateDataGrid();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsItemSelected())
            {
                return;
            }

            bool success = itemService.DeleteItem(selectedItem.Id, user.LoginToken.SecureToken);
            if (!success)
            {
                MessageBox.Show("Something went wrong! Try again!");
                return;
            }

            ResetForm();
            UpdateDataGrid();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!IsItemSelected())
            {
                return;
            }

            FillFormWithItem(selectedItem);
        }
    }
}
