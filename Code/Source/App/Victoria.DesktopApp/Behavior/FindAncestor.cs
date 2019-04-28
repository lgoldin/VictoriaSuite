using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Victoria.DesktopApp.Behavior
{
    public static class FindAncestor
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        public static T FindAncestorMethod<T>(this DependencyObject obj) where T : DependencyObject
        {
            logger.Info("Inicio Encontrar Ancestro");
            DependencyObject tmp = VisualTreeHelper.GetParent(obj);
            while (tmp != null && !(tmp is T))
            {
                tmp = VisualTreeHelper.GetParent(tmp);
            }
            logger.Info("Fin Encontrar Ancestro");

            return tmp as T;
        }
    }
}
