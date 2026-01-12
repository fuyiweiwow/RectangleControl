using System.Windows;
using System.Windows.Media;

namespace RectangleControl.Utils
{
     public static class Utils
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
            {
                yield break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T t)
                {
                    yield return t;
                }

                foreach (T item in child.FindVisualChildren<T>())
                {
                    yield return item;
                }
            }
        }

        public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
            {
                return null;
            }

            if (parentObject is T parent)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

    }
}