using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace test
{
    public partial class MainPage : ContentPage
    {
        private readonly HomeViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new HomeViewModel();
            BindingContext = _viewModel;
        }

        public async Task Init()
        {
            await _viewModel.RequestPermissions();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
      

        }

    }
}
