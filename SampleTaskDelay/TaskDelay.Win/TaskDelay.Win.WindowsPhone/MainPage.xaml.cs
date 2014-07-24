using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace TaskDelay.Win
{
    /// <summary>
    /// Frame 内へナビゲートするために利用する空欄ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now;
        TimeSpan span = new TimeSpan();
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.buttonStart.Click += buttonStart_Click;
            this.buttonStop.Click += buttonStop_Click;
            this.buttonReset.Click += buttonReset_Click;
        }

        void display()
        {
            this.span = endTime - startTime;


            this.textTime.Text = string.Format("{0:00}min {1:00}sec {2:000}",
                span.Minutes, span.Seconds, span.Milliseconds);
        }

        void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            this.startTime = this.endTime = DateTime.Now;
            display();
        }

        void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            timer.Stop();
            this.endTime = DateTime.Now;
            display();
        }

        bool flag = false;

        void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            this.startTime = DateTime.Now;
            this.endTime = this.startTime;

            // これは動かない
            /*
            var t = Task.Factory.StartNew(
                async () =>
                {
                    flag = true;
                    while (flag)
                    {
                        this.endTime = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine("time {0}", endTime);
                        display();
                        await Task.Delay(1000);
                    }
                }
                );
             */
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (_, __) =>
            {
                this.endTime = DateTime.Now;
                System.Diagnostics.Debug.WriteLine("time {0}", endTime);
                display();
            };
            timer.Start();

        }


        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。
        /// このプロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: ここに表示するページを準備します。

            // TODO: アプリケーションに複数のページが含まれている場合は、次のイベントの
            // 登録によりハードウェアの戻るボタンを処理していることを確認してください:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed イベント。
            // 一部のテンプレートで指定された NavigationHelper を使用している場合は、
            // このイベントが自動的に処理されます。
        }
    }
}
