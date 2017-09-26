using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading;
using Android.Media;

namespace KitchenFinks
{
    [Activity(Label = "KitchenFinks", MainLauncher = true)]
    public class MainActivity : Activity
    {
        // 残り時間を管理するプライベートフィールド
        private TimeSpan _remainingTime = new TimeSpan(0);

        // カウントダウン用のタイマー
        private Timer _timer;

        // タイマーが起動しているかどうかのフラグ
        // 最初は起動してないのでfalse
        private bool _isStart = false;

        /// <summary>
        /// メインエントリ
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // 10分ボタンのイベントハンドラ
            FindViewById<Button>(Resource.Id.Add10MinButton).Click += (s, e) =>
            {
                // 10分追加
                _remainingTime = _remainingTime.Add(TimeSpan.FromMinutes(10));
                // TextViewに反映させる
                ShowRemainingTime();
            };

            // 1分ボタンのイベントハンドラ
            FindViewById<Button>(Resource.Id.Add1MinButton).Click += (s, e) =>
            {
                // 1分追加
                _remainingTime = _remainingTime.Add(TimeSpan.FromMinutes(1));
                // Viewに反映
                ShowRemainingTime();
            };

            // 10秒ボタンのイベントハンドラ
            FindViewById<Button>(Resource.Id.Add10SecButton).Click += (s, e) =>
            {
                // 10秒追加
                _remainingTime = _remainingTime.Add(TimeSpan.FromSeconds(10));
                // Viewに反映
                ShowRemainingTime();
            };

            // 1秒ボタンのイベントハンドラ
            FindViewById<Button>(Resource.Id.Add1SecButton).Click += (s, e) =>
            {
                // 1秒追加
                _remainingTime = _remainingTime.Add(TimeSpan.FromSeconds(1));
                // Viewに反映
                ShowRemainingTime();
            };

            // クリアボタンのイベントハンドラ
            FindViewById<Button>(Resource.Id.ClearButton).Click += (s, e) =>
            {
                // クリア
                _remainingTime = new TimeSpan(0);
                // Viewに反映
                ShowRemainingTime();
            };

            // スタートボタンのイベントハンドラ
            FindViewById<Button>(Resource.Id.StartButton).Click += (s, e) =>
            {
                // 初期値がfalseなので反転させる
                // 命名的にわかりやすさ重視
                _isStart = !_isStart;

                if (_isStart)
                    // 起動してない→起動なので
                    // ボタンの表示をスタート→ストップに
                    FindViewById<Button>(Resource.Id.StartButton).Text = "ストップ";
                else
                    // 起動している→止めるなので
                    // ボタンの表示をストップ→スタートに
                    FindViewById<Button>(Resource.Id.StartButton).Text = "スタート";
            };

            // タイマーにコールバックされた残り時間を入れる
            _timer = new Timer(Timer_OnTick, null, 0, 100);

        }

        /// <summary>
        /// タイマーのコールバックメソッド
        /// </summary>
        /// <param name="state"></param>
        private void Timer_OnTick(object state)
        {
            // 起動してるか？
            if (!_isStart)
                return;

            // UIスレッドで動作
            RunOnUiThread(() =>
            {
                // 1秒減らす
                _remainingTime = _remainingTime.Add(TimeSpan.FromMilliseconds(-100));
                // 残りが0
                if (_remainingTime.TotalSeconds <= 0)
                {
                    // 起動をやめる
                    _isStart = false;
                    // クリア
                    _remainingTime = new TimeSpan(0);
                    // ボタンの表示をストップ→スタートに
                    FindViewById<Button>(Resource.Id.StartButton).Text = "スタート";
                    // アラーム
                    // ボリュームを1にしてある
                    new ToneGenerator(Stream.System, 50).StartTone(Tone.PropBeep);
                }
                // Viewに反映
                ShowRemainingTime();
            });
        }

        /// <summary>
        /// TextViewに反映させる
        /// </summary>
        private void ShowRemainingTime() => FindViewById<TextView>(Resource.Id.RemainingTimeTextView).Text
                = string.Format("{0:f0}:{1:d2}", _remainingTime.Minutes, _remainingTime.Seconds);



    }
}

