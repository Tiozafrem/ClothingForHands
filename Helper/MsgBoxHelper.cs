using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClothingForHands.Helper
{
    class MsgBoxHelper
    {
        public void Error(Exception exception)
        {
            MessageBox.Show(exception.Message, exception.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void Error(string text, string title = "Ошибка")
        {
            MessageBox.Show(text,title,MessageBoxButton.OK,MessageBoxImage.Error);
        }
        public void Information(string text, string title = "Информация")
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public bool Question(string text, string title = "Вопрос")
        {
            return MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
        public void Warning(string text, string title = "Предупреждение")
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
