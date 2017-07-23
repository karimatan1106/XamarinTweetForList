using System;
using CoreTweet;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace XamarinTweetForList.Models
{
    public class Tweet
    {
        #region 認証パラメータ
        private const string Consumer_Key = "Your ConsumerKey";
        private const string Consumer_Secret = "Your ConsumerSecuret";
        private const string Access_Token = "Your AccessToken";
        private const string Access_Token_Secret = "Your AccessTokenSecret";
        #endregion

        #region プロパティ
        public ObservableCollection<long> UserListIds { get; set; }
        public ObservableCollection<Timeline> Timeline { get; set; }
        #endregion

        #region フィールド
        private readonly Tokens _tokens;
        #endregion

        #region コンストラクタ
        public Tweet()
        {
            UserListIds = new ObservableCollection<long>();
            Timeline = new ObservableCollection<Timeline>();

            // API にアクセスするためのトークン群
            _tokens = Tokens.Create(
                    Consumer_Key,
                    Consumer_Secret,
                    Access_Token,
                    Access_Token_Secret);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 指定したユーザが使用可能なリストIDを取得
        /// </summary>
        /// <param name="screenName"></param>
        /// <returns></returns>
        public async Task GetListIdsAsync(string screenName)
        {
            //ListIDの初期化
            UserListIds.Clear();

            //List一覧の取得
            var userLists = await _tokens
                .Lists
                .ListAsync(screen_name => screenName);

            //ListIDの取得
            foreach (var list in userLists)
            {
                UserListIds.Add(list.Id);
            }
        }
        /// <summary>
        /// 指定したリストIDのタイムラインを取得
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public async Task GetListTimelineAsync(long listID)
        {
            //タイムラインの初期化
            Timeline.Clear();

            //指定したリストのタイムラインを取得
            var listStatuses = await _tokens
                .Lists
                .StatusesAsync(list_id => listID,
                               count => 100,
                               include_rts => true);

            //取得したタイムラインから必要な情報を抽出
            foreach (var tweet in listStatuses)
            {
                var createdAtUTC = new DateTimeOffset(tweet.CreatedAt.Ticks, TimeSpan.Zero);
                var createdAtJST = TimeZoneInfo.ConvertTime(createdAtUTC, TimeZoneInfo.Local);
                var postedAt = GetPostedAt(DateTime.Now - createdAtJST);

                //タイムラインに追加
                Timeline.Add(
                    new Timeline
                    {
                        ProfileImageURL = tweet.User.ProfileImageUrl.Replace("_normal", ""),
                        Name = tweet.User.Name,
                        ScreenName = tweet.User.ScreenName,
                        DisplayScreenName = $"@{tweet.User.ScreenName}",
                        Text = tweet.Text,
                        CreatedAt = createdAtJST,
                        PostedAt = postedAt,
                        IsRetweeted = tweet.IsRetweeted == true,
                        IsFavorited = tweet.IsFavorited == true,
                        RetweetCount = tweet.RetweetCount,
                        FavoriteCount = tweet.FavoriteCount,
                    });
            }
        }

        /// <summary>
        /// 現在日時と投稿日時の差からいつ投稿したかを返す
        /// </summary>
        /// <param name="difAt"></param>
        /// <returns></returns>
        private static string GetPostedAt(TimeSpan difAt)
        {
            if (difAt.Days > 0)
            {
                //x月y日の形式で返す
                return DateTime.Now.ToString("m");
            }
            if (difAt.Hours > 0)
            {
                return $"{difAt.Hours}時間前";
            }
            if (difAt.Minutes > 0)
            {
                return $"{difAt.Minutes}分前";
            }
            return $"{difAt.Seconds}秒前";
        }

        #endregion
    }
}