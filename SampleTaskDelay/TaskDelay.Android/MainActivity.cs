using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;

namespace TaskDelay.Android
{
    [Activity(Label = "TaskDelay.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        TextView textTime;
        Button buttonStart, buttonStop, buttonReset;

        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now;
        TimeSpan span = new TimeSpan();
        System.Timers.Timer timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            this.textTime = FindViewById<TextView>(Resource.Id.textTime);
            this.buttonStart = FindViewById<Button>(Resource.Id.buttonStart);
            this.buttonStop = FindViewById<Button>(Resource.Id.buttonStop);
            this.buttonReset = FindViewById<Button>(Resource.Id.buttonReset);

            this.buttonStart.Click += buttonStart_Click;
            this.buttonStop.Click += buttonStop_Click;
            this.buttonReset.Click += buttonReset_Click;
            
        }

        void buttonReset_Click(object sender, EventArgs e)
        {
            this.startTime =  this.endTime = DateTime.Now;
            display();
        }

        Handler mh = new Handler();

        void display()
        {
            this.span = endTime - startTime;
            // メインスレッドの更新が必須
            mh.Post(() =>
            {
                this.textTime.Text = string.Format("{0:00}min {1:00}sec {2:000}",
                    span.Minutes, span.Seconds, span.Milliseconds);
            });
        }

        void buttonStop_Click(object sender, EventArgs e)
        {
            flag = false;
            timer.Stop();

            this.endTime = DateTime.Now;
            display();
        }

        bool flag = false;

        void buttonStart_Click(object sender, EventArgs e)
        {
            this.startTime = DateTime.Now;
            this.endTime = this.startTime;

            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.AutoReset = true;
            timer.Elapsed += (_, __) =>
            {
                this.endTime = DateTime.Now;
                System.Diagnostics.Debug.WriteLine("time {0}", endTime);
                display();
            };
            timer.Start();
            /*
             * これは、まともに動かない
             */
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
        }

    }
}

