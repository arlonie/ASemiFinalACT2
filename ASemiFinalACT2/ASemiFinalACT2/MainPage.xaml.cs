using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ASemiFinalACT2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Get All Persons  
            var ordersList = await App.SQLiteDb.DisplayAll();
            if (ordersList != null)
            {
                Orders.ItemsSource = ordersList;
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            double priceAdd;
            string addOn = "";
            if (!string.IsNullOrEmpty(Addons.Text))
            {
                Double.TryParse(PriceAddons.Text, out priceAdd);
                addOn = Addons.Text;
            }
            else
            {
                priceAdd = 0;
            }

            string entrees = "";
            if(Option1.SelectedIndex == 0)
            {
                entrees = "steak, salmon";
            }else if(Option1.SelectedIndex == 1)
            {
                entrees = "vegetarian past";
            }

            string sides = "";
            if (Option21.IsChecked)
            {
                sides = "mashed potatoes, grilled asparagus";
            }else if (Option22.IsChecked)
            {
                sides = "garlic bread";
            }

            string dessert = "";
            if (Option31.IsChecked && Option32.IsChecked)
            {
                dessert = "lava cake, tiramisu";
            }else if (Option31.IsChecked)
            {
                dessert = "lava cake";
            }
            else if (Option32.IsChecked)
            {
                dessert = "tiramisu";
            }

            double dinnerpackage = 1500;
            double total = dinnerpackage + priceAdd;

            ORDERS orders = new ORDERS()
            {
                ENTREES = entrees,
                SIDES = sides,
                DESSERT = dessert,
                ADD_ON = addOn,
                TOTAL = total
            };

            await App.SQLiteDb.Save(orders);
            Addons.Text = string.Empty;
            PriceAddons.Text = string.Empty;
            Option21.IsChecked = false;
            Option22.IsChecked = false;
            Option31.IsChecked = false;
            Option32.IsChecked = false;
            Option1.SelectedIndex = -1;
            await DisplayAlert("Success", "Added Successfully", "Ok");

            var ordesList = await App.SQLiteDb.DisplayAll();
            if (ordesList != null)
            {
                Orders.ItemsSource = ordesList;
            }


        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar.Text))
            {
                if (int.TryParse(SearchBar.Text, out int search_num))
                {
                    var salesNum = await App.SQLiteDb.Search(search_num);
                    if (salesNum != null)
                    {
                        // Store the original values
                        string originalEntrees = salesNum.ENTREES;
                        string originalSides = salesNum.SIDES;
                        string originalDessert = salesNum.DESSERT;
                        string originalAddOn = salesNum.ADD_ON;
                        double originalTotal = salesNum.TOTAL;

                        double priceAdd;
                        string addOn = "";
                        if (!string.IsNullOrEmpty(Addons.Text))
                        {
                            double.TryParse(PriceAddons.Text, out priceAdd);
                            salesNum.ADD_ON = Addons.Text;
                        }
                        else
                        {
                            priceAdd = 0;
                        }

                        string entrees = "";
                        if (Option1.SelectedIndex == 0)
                        {
                            entrees = "steak, salmon";
                        }
                        else if (Option1.SelectedIndex == 1)
                        {
                            entrees = "vegetarian past";
                        }

                        string sides = "";
                        if (Option21.IsChecked)
                        {
                            sides = "mashed potatoes, grilled asparagus";
                        }
                        else if (Option22.IsChecked)
                        {
                            sides = "garlic bread";
                        }

                        string dessert = "";
                        if (Option31.IsChecked && Option32.IsChecked)
                        {
                            dessert = "lava cake, tiramisu";
                        }
                        else if (Option31.IsChecked)
                        {
                            dessert = "lava cake";
                        }
                        else if (Option32.IsChecked)
                        {
                            dessert = "tiramisu";
                        }

                        // Recalculate total only if addons price has changed
                        if (salesNum.ADD_ON != originalAddOn)
                        {
                            double dinnerpackage = 1500;
                            salesNum.TOTAL = dinnerpackage + priceAdd;
                        }
                        else
                        {
                            // Restore the original total if addons price hasn't changed
                            salesNum.TOTAL = originalTotal;
                        }

                        // Save the updated record
                        await App.SQLiteDb.Save(salesNum);
                        await DisplayAlert("Success", "Order Updated Successfully", "OK");

                        // Refresh records list
                        var ordersList = await App.SQLiteDb.DisplayAll();
                        if (ordersList != null)
                        {
                            Orders.ItemsSource = ordersList;
                        }

                        SearchBar.Text = string.Empty;
                        Addons.Text = string.Empty;
                        PriceAddons.Text = string.Empty;
                        Option21.IsChecked = false;
                        Option22.IsChecked = false;
                        Option31.IsChecked = false;
                        Option32.IsChecked = false;
                        Option1.SelectedIndex = -1;
                    }
                    else
                    {
                        await DisplayAlert("Not Found", "Sales Order No. not found.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Sales Order No. must be a valid integer.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter Sales Order No.", "OK");
            }
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar.Text))
            {
                if (int.TryParse(SearchBar.Text, out int search_num))
                {
                    var salesNum = await App.SQLiteDb.Search(search_num);
                    if (salesNum != null)
                    {
                        await App.SQLiteDb.Delete(salesNum);
                        SearchBar.Text = string.Empty;
                        await DisplayAlert("Success", "Order Delete", "OK");

                        var ordersList = await App.SQLiteDb.DisplayAll();
                        if (ordersList != null)
                        {
                            Orders.ItemsSource = ordersList;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Not Found", "Sales Order No. not found.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Sales Order No. must be a valid integer.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter Sales Order No.", "OK");
            }
        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBar.Text))
            {
                if (int.TryParse(SearchBar.Text, out int search_num))
                {
                    var salesNum = await App.SQLiteDb.Search(search_num);
                    if (salesNum != null)
                    {
                        // Clear previous items in the ListView
                        Orders.ItemsSource = null;

                        // Create a list with the searched record
                        List<ORDERS> searchResult = new List<ORDERS>();
                        searchResult.Add(salesNum);

                        // Set the ListView's ItemsSource to display the searched record
                        Orders.ItemsSource = searchResult;

                        await DisplayAlert("Success",
                            "Sales Oder No.: " + salesNum.SONum + "\n" +
                            "Entrees: " + salesNum.ENTREES + "\n" +
                            "Sides: " + salesNum.SIDES + "\n" +
                            "Dessert: " + salesNum.DESSERT + "\n" +
                            "Add on: " + salesNum.ADD_ON + "\n" +
                            "Total: " + salesNum.TOTAL,
                            "OK");
                    }
                    else
                    {
                        await DisplayAlert("Not Found", "Sales Order No. not found.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Sales Order No. must be a valid integer.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter Sales Order No.", "OK");
            }

        }
    }
}
