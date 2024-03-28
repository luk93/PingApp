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

namespace PingApp.Controls
{
    /// <summary>
    /// Interaction logic for ProgressBarPercent.xaml
    /// </summary>
    public partial class ProgressBarPercent : UserControl
    {
        public ProgressBarPercent()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(ProgressBarPercent_Loaded);
        }

        void ProgressBarPercent_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(ProgressBarPercent), new PropertyMetadata(100d, OnMaximumChanged));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }


        private static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(ProgressBarPercent), new PropertyMetadata(0d, OnMinimumChanged));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ProgressBarPercent), new PropertyMetadata(50d, OnValueChanged));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static readonly DependencyProperty ProgressBarWidthProperty = DependencyProperty.Register("ProgressBarWidth", typeof(double), typeof(ProgressBarPercent), null);
        private double ProgressBarWidth
        {
            get { return (double)GetValue(ProgressBarWidthProperty); }
            set { SetValue(ProgressBarWidthProperty, value); }
        }

        private static readonly DependencyProperty PercentProperty = DependencyProperty.Register("Percent", typeof(double), typeof(ProgressBarPercent), null);
        private double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarPercent).Update();
        }

        static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarPercent).Update();
        }

        static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as ProgressBarPercent).Update();
        }

        void Update()
        {
            var pbWidth = Math.Min((Value / (Maximum + Minimum) * this.ActualWidth) - 2, this.ActualWidth - 2);
            ProgressBarWidth = pbWidth < 0 ? 0 : pbWidth;
            Percent = Math.Min((int)(Value / (Maximum + Minimum) * 100), 100);
        }
    }
}
