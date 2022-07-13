using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorPage : ContentPage
    {
        public ErrorPage()
        {
            InitializeComponent();
        }

        public ErrorPage(Exception exception)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(exception.Message))
                    label.Text = exception.Message.ToString();
        }

        public ErrorPage(string Error)
        {
            InitializeComponent();
            label.Text = Error;
        }
    }
}