using System;
using Xamarin.Forms;

namespace XamarinTweetForList.Models
{
    public class Timeline
    {
        public string ProfileImageURL { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string DisplayScreenName { get; set; }
        public string Text { get; set; }
        public string TweetedTime { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string PostedAt { get; set; }
        public bool IsRetweeted { get; set; }
        public bool IsFavorited { get; set; }
        public bool IsQuotedStatus { get; set; }
        public int? RetweetCount { get; set; }
        public int? FavoriteCount { get; set; }
        public ImageSource FavoriteImage { get; set; }
    }
}
