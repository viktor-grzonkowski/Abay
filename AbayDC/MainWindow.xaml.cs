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
using System.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DedicatedCliend.ItemServiceReference;

namespace DedicatedClient
{
    public partial class MainWindow : Window
    {
        private ItemServiceClient itemService;
        private ObservableCollection<Item> items;
        private Item selectedItem;
        private User user;

        public MainWindow(User user)
        {
            InitializeComponent();

            itemService = new ItemServiceClient("NetTcpBinding_IItemService");

            this.user = user;

            //Fill up DataContext for DataGrid
            items = new ObservableCollection<Item>(itemService.GetAllItems(-1));
            dgItems.DataContext = items;

            selectedItem = null;
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
            //itemService.UpdateItem(selectedItem.Id, user.);
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FillFormWithItem(selectedItem);
        }
    }
}
