using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PingApp.Controls
{
    /// <summary>
    /// Interaction logic for ProgressBarTimeout.xaml
    /// </summary>
    public partial class ProgressBarTimeout : UserControl
    {
        private DispatcherTimer _timerUpdate;
        private DispatcherTimer _timerCheck;
        private readonly int _interval = 20;

        public ProgressBarTimeout()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ProgressBarTimeout_Loaded);
        }

        void ProgressBarTimeout_Loaded(object sender, RoutedEventArgs e)
        {
            Value = 0;
            Maximum = 1;
            ProgressBarWidth = 0;
        }

        private static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(ProgressBarTimeout), new PropertyMetadata(100d, OnMaximumChanged));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }


        private static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(ProgressBarTimeout), new PropertyMetadata(0d, OnMinimumChanged));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ProgressBarTimeout), new PropertyMetadata(0d, OnValueChanged));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static readonly DependencyProperty ProgressBarWidthProperty = DependencyProperty.Register("ProgressBarWidth", typeof(double), typeof(ProgressBarTimeout), null);
        private double ProgressBarWidth
        {
            get { return (double)GetValue(ProgressBarWidthProperty); }
            set { SetValue(ProgressBarWidthProperty, value); }
        }

        private static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register("Timeout", typeof(int), typeof(ProgressBarTimeout), new PropertyMetadata(3000, OnTimeoutChanged));
        public int Timeout
        {
            get { return (int)GetValue(TimeoutProperty); }
            set { SetValue(TimeoutProperty, value); }
        }

        private static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(ProgressBarTimeout), null);
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarTimeout).Update();
        }

        static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarTimeout).Update();
        }

        static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarTimeout).Update();
        }

        static void OnTimeoutChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarTimeout).StartTimers();
        }

        private void StartTimers()
        {
            StopTimers();

            Maximum = Timeout;

            _timerUpdate = new DispatcherTimer();
            _timerUpdate.Interval = TimeSpan.FromMilliseconds(_interval);
            _timerUpdate.Tick += TimerUpdate_Tick;
            _timerUpdate.Start();

            _timerCheck = new DispatcherTimer();
            _timerCheck.Interval = TimeSpan.FromMilliseconds(_interval);
            _timerCheck.Tick += TimerCheck_Tick;
            _timerCheck.Start();

            Value = 0;
            ProgressBarWidth = 0;
        }

        private void StopTimers()
        {
            if (_timerUpdate != null)
            {
                _timerUpdate.Stop();
                _timerUpdate.Tick -= TimerUpdate_Tick;
            }

            if (_timerCheck != null)
            {
                _timerCheck.Stop();
                _timerCheck.Tick -= TimerCheck_Tick;
            }
        }

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            Value += _interval*2;
        }

        private void TimerCheck_Tick(object sender, EventArgs e)
        {
            if (Value >= Maximum)
            {
                StopTimers();
                Value = 0;
            }
        }

        private void Update()
        {
            var pbWidth = Math.Min((Value / (Maximum + Minimum) * ActualWidth) - 2, ActualWidth - 2);
            ProgressBarWidth = pbWidth < 0 ? 0 : pbWidth;
        }
    }
}
