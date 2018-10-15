using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Microsoft.Azure.Documents.Client;

namespace MarketingMovie
{
    public partial class MainPage : ContentPage
    {
        IAuthService authService;
        Microsoft.Identity.Client.AuthenticationResult authResult;

        public MainPage()
        {
            InitializeComponent();

            authService = DependencyService.Get<IAuthService>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //getMovies.Clicked += GetMovies;

            login.Clicked += Login;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //getMovies.Clicked -= GetMovies;

            login.Clicked -= Login;
        }

        //async void GetMovies(object sender, EventArgs args)
        //{
        //    //var dataService = new CosmosService();

        //    //var reviews = await dataService.GetReviewsForMovie("b404bb8f-f9c9-4389-9fa6-26aca20104fe");

        //    //var sb = new StringBuilder();
        //    //foreach (var item in reviews)
        //    //{
        //    //    sb.AppendLine(item.reviewText);
        //    //}

        //    //theMovies.Text = sb.ToString();
        //}

        async void Login(object sender, EventArgs args)
        {
            authResult = await authService.Login();
        }

        async void GetPremium_Clicked(object sender, System.EventArgs e)
        {
            // Get reviews for a movie
            //var token = authResult?.IdToken;

            //bool auth = !string.IsNullOrEmpty(token);

            //var functionSvc = new FunctionsService();

            //var cosmosToken = await functionSvc.GetCosmosPermissionToken(token, false);

            //// non-premium: 251a5359-42ef-4a68-9f34-919c2d694a0d
            //// premium: b404bb8f-f9c9-4389-9fa6-26aca20104fe

            //var movieId = "b404bb8f-f9c9-4389-9fa6-26aca20104fe";

            //var cosmosService = new CosmosService();

            //var reviews = await cosmosService.GetReviewsForMovie(movieId, auth, cosmosToken);

            //var sb = new StringBuilder();
            //foreach (var item in reviews)
            //{
            //    sb.AppendLine(item.reviewText);
            //}

            //theMovies.Text = sb.ToString();
        }

        async void GetPublic_Clicked(object sender, System.EventArgs e)
        {
            // get all movies
            //var fs = new FunctionsService();
            //var cosmosToken = await fs.GetCosmosPermissionToken("", true);

            //var cosmosService = new CosmosService();

            //var movies = await cosmosService.GetAllMovies(cosmosToken);

            //var sb = new StringBuilder();
            //foreach (var item in movies)
            //{
            //    sb.AppendLine(item.movieName);
            //}

            //theMovies.Text = sb.ToString();


        }
    }
}
