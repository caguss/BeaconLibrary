using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test
{
    public partial class App : Application
    {
        private readonly MainPage _viewInstance;

        public App()
        {
            InitializeComponent();

            _viewInstance = new MainPage();
            MainPage = _viewInstance;
        }

        protected override async void OnStart()
        {
            await _viewInstance.Init();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
