using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MarketingMovie
{
    public partial class ReviewListPage : ContentPage
    {
        string MovieId;

        public ReviewListPage(string movieId, string movieName)
        {
            InitializeComponent();

            Title = movieName;
            MovieId = movieId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var cosmosService = new CosmosService();
            var reviews = await cosmosService.GetReviewsForMovie(MovieId);

            reviewListView.ItemsSource = reviews;
        }

    }
}
