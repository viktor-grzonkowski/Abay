using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DedicatedClient.ItemServiceReference;

namespace DedicatedClient
{
    public partial class MainWindow : Window
    {
        private ItemServiceClient itemService;
        private BackgroundWorker updateWorker;
        private Item selectedItem;
        private UserServiceReference.User user;
        private BackgroundWorker searchWorker;
        private string lastKeyword;

        public MainWindow(UserServiceReference.User user)
        {
            InitializeComponent();

            itemService = new ItemServiceClient("NetTcpBinding_IItemService");
            this.user = user;

            updateWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = false,
                WorkerReportsProgress = false
            };
            updateWorker.DoWork += UpdateWorker_DoWork;
            updateWorker.RunWorkerCompleted += UpdateWorker_RunWorkerCompleted;
            UpdateDataGrid();

            selectedItem = null;

            searchWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = false,
                WorkerReportsProgress = false
            };
            searchWorker.DoWork += SearchWorker_DoWork;
            searchWorker.RunWorkerCompleted += SearchWorker_RunWorkerCompleted;
            lastKeyword = "";
        }
   
        private void UpdateDataGrid()
        {
            if (updateWorker.IsBusy) return;

            updateWorker.RunWorkerAsync();
        }
        private void UpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Item[] results = itemService.GetAllItems();
            e.Result = new ObservableCollection<Item>(results);
        }
        private void UpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgItems.DataContext = (ObservableCollection<Item>)e.Result;
        }

        private void Search()
        {
            if (searchWorker.IsBusy) return;

            string keyword = txtSearch.Text.Trim();
            if (keyword == lastKeyword) return;

            lastKeyword = keyword;

            searchWorker.RunWorkerAsync(keyword);
        }
        private void SearchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string keyword = (string)e.Argument;
            Item[] results = itemService.SearchItems(keyword, -1);
            e.Result = new ObservableCollection<Item>(results);
        }
        private void SearchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgItems.DataContext = (ObservableCollection<Item>)e.Result;

            //check if there was another request submitted, while it was searching
            Search();
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

            lvBids.DataContext = itemService.GetAllBidsByItem(item.Id);
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

            itemService.UpdateItem(selectedItem.Id, user.LoginToken.SecureToken, txtName.Text,
                 txtDescribe.Text, selectedItem.Category.Id);

            UpdateDataGrid();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsItemSelected())
            {
                return;
            }

            itemService.DeleteItem(selectedItem.Id, user.LoginToken.SecureToken);

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
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
