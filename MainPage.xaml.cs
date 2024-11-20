using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MoneyCounter
{
    public partial class MainPage : ContentPage
    {
        private decimal totalSum = 0;
        private const decimal Limit = 1000;
        private const string TotalSumKey = "TotalSum";

        public MainPage()
        {
            InitializeComponent();
            LoadState();
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            // Try to parse the input number
            if (decimal.TryParse(NumberEntry.Text, out decimal number))
            {
                // Check if adding the number would exceed the limit
                if (totalSum + number > Limit)
                {
                    // Inform the user that the number is too big
                    DisplayAlert("Number Too Big", "The number you entered would exceed the limit of 1000.", "OK");
                }
                else
                {
                    // Add the number to the total sum
                    totalSum += number;

                    // Update the label
                    SumLabel.Text = $"Sum: {totalSum}";

                    // Clear the input box
                    NumberEntry.Text = string.Empty;

                    // Save the updated state
                    SaveState();

                    // Check if the sum has reached or exceeded the limit
                    if (totalSum >= Limit)
                    {
                        // Disable the input box
                        NumberEntry.IsEnabled = false;

                        // Optionally, change the background color
                        this.BackgroundColor = Colors.LightGreen;

                        // Optionally, change the text color
                        SumLabel.TextColor = Colors.Red;

                        // Display a message
                        SumLabel.Text += "\nTarget reached!";
                    }
                }
            }
            else
            {
                // Inform the user about invalid input
                DisplayAlert("Invalid Input", "Please enter a valid number.", "OK");
            }
        }

        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            // Reset the total sum
            totalSum = 0;

            // Update the label
            SumLabel.Text = $"Sum: {totalSum}";

            // Enable the input box
            NumberEntry.IsEnabled = true;

            // Reset background and text colors if changed
            this.BackgroundColor = Colors.White;
            SumLabel.TextColor = Colors.Black;

            // Save the updated state
            SaveState();
        }

        private void SaveState()
        {
            Preferences.Set(TotalSumKey, (double)totalSum);
        }

        private void LoadState()
        {
            if (Preferences.ContainsKey(TotalSumKey))
            {
                totalSum = (decimal)Preferences.Get(TotalSumKey, 0.0);
                SumLabel.Text = $"Sum: {totalSum}";

                // Check if the sum has reached or exceeded the limit
                if (totalSum >= Limit)
                {
                    NumberEntry.IsEnabled = false;
                    this.BackgroundColor = Colors.LightGreen;
                    SumLabel.TextColor = Colors.Red;
                    SumLabel.Text += "\nTarget reached!";
                }
            }
        }
    }
}