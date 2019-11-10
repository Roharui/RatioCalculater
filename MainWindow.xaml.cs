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
using System.Text.RegularExpressions;

namespace RatioCalculater
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Regex regex = new Regex("[^0-9]+");

        private int unit = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Text = "0";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            unit = Summary_AllUnit();
            Unit.Text = unit.ToString() + " unit";

            /*
            for(var i = 1;i < 5; i++)
            {
                Console.WriteLine(Calc_Ratio(i));
            }
            */

            ratio_1.Text = String.Format("{0:F1}%", Calc_Ratio(1));
            ratio_2.Text = String.Format("{0:F1}%", Calc_Ratio(2));
            ratio_3.Text = String.Format("{0:F1}%", Calc_Ratio(3));
            ratio_4.Text = String.Format("{0:F1}%", Calc_Ratio(4));
        }

        private int Summary_AllUnit()
        {
            int result = 0;
            foreach(TextBox tb in FindVisualChildren<TextBox>(this))
            {
                result += Get_Unit(tb);
            }
            return result;
        }

        private int Get_UnitByLine(int line_num)
        {
            int result = 0;
            foreach (TextBox tb in FindVisualChildren<TextBox>(this))
            {
                String name = tb.Name;
                if(name.Contains(line_num.ToString()))
                {
                    result += Get_Unit(tb);
                }
            }
            return result;
        }

        private int Get_Unit(TextBox tb)
        {
            String name = tb.Name;
            if (tb.Text == "") return 0;
            if (name.Contains("poor"))
            {
                return Int32.Parse(tb.Text) * 15;
            }
            else if (name.Contains("normal"))
            {
                return Int32.Parse(tb.Text) * 25;
            }
            else if (name.Contains("rich"))
            {
                return Int32.Parse(tb.Text) * 35;
            }
            return 0;
        }

        private double Calc_Ratio(int num)
        {
            if(unit == 0) { return 0.0; }
            Console.WriteLine(Get_UnitByLine(num));
            return (Get_UnitByLine(num) / (double)unit) * 100;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
