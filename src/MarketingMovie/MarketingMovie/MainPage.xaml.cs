using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace MarketingMovie
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            getMovies.Clicked += GetMovies;

            login.Clicked += Login;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            getMovies.Clicked -= GetMovies;

            login.Clicked -= Login;
        }

        async void GetMovies(object sender, EventArgs args)
        {
            var dataService = new CosmosService();

            var reviews = await dataService.GetReviewsForMovie("b404bb8f-f9c9-4389-9fa6-26aca20104fe");

            var sb = new StringBuilder();
            foreach (var item in reviews)
            {
                sb.AppendLine(item.reviewText);
            }

            theMovies.Text = sb.ToString();
        }

        async void Login(object sender, EventArgs args)
        {
            var authService = new AuthenticationService();

            var result = await authService.Login();
        }
    }
}
