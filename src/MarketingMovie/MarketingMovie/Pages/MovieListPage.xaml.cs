using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace MarketingMovie
{
    public partial class MovieListPage : ContentPage
    {
        public MovieListPage()
        {
            InitializeComponent();

            Title = "The Movies!";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (movieListView.ItemsSource == null)
            {
                var cosmosService = new CosmosService();

                var movies = await cosmosService.GetAllMovies();

                movieListView.ItemsSource = movies;
            }

            movieListView.ItemSelected += MovieListView_ItemSelected;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            movieListView.ItemSelected -= MovieListView_ItemSelected;
        }

        async void MovieListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var movie = e.SelectedItem as Movie;

            if (movie == null)
                return;

            var reviewPage = new ReviewListPage(movie.id, movie.movieName);

            await Navigation.PushAsync(reviewPage);

            movieListView.SelectedItem = null;
        }


        async void Login_Clicked(object sender, System.EventArgs e)
        {
            var idService = DependencyService.Get<IAuthService>();

            var res = await idService.Login();
        }


    }
}
