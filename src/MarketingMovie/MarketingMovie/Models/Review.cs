using System;

namespace MarketingMovie
{
    public class Review
    {
        public Review()
        {
        }

        public string id { get; set; }

        public string movieId { get; set; }

        public bool isPremium { get; set; }

        public string reviewText { get; set; }

        public string reviewDate { get; set; }
    }
}
