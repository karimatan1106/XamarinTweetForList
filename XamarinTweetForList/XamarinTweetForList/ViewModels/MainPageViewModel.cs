using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XamarinTweetForList.Models;

namespace XamarinTweetForList.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigatedAware
    {
        #region プロパティ
        public ObservableCollection<string> TweetList { get; set; }
        public ObservableCollection<long> UserListIds { get; set; }
        public ObservableCollection<Timeline> Timeline { get; set; }
        #endregion

        #region フィールド
        private readonly Tweet _tweet = new Tweet();
        private readonly string _screenName;
        #endregion

        #region コンストラクタ
        public MainPageViewModel()
        {
            TweetList = new ObservableCollection<string>();
            UserListIds = new ObservableCollection<long>();
            Timeline = new ObservableCollection<Timeline>();
            _screenName = "OXamarin";
        }
        #endregion

        #region メソッド

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            //ignore
        }
        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            await GetListAsync(_screenName);
        }
        /// <summary>
        /// 指定したユーザの最初のListを表示する
        /// </summary>
        /// <returns></returns>
        private async Task GetListAsync(string screenName)
        {
            //指定したユーザに公開されているListのListIDを取得
            await _tweet.GetListIdsAsync(screenName);
            UserListIds = _tweet.UserListIds;

            //ListIDがあれば最初のListをTimelineに表示する
            if (UserListIds.Any())
            {
                //タイムラインの初期化
                Timeline.Clear();

                //最初のListに含まれるツイートを取得
                await _tweet.GetListTimelineAsync(UserListIds.First());

                //取得したツイートをタイムラインへ反映
                foreach (var tweet in _tweet.Timeline)
                {
                    Timeline.Add(tweet);
                }
            }
        }
        #endregion
    }
}
