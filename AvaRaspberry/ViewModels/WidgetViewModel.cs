using AvaRaspberry.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaRaspberry.ViewModels
{
    public abstract class WidgetViewModel : ViewModelBase, IWidgetViewModel
    {
        public abstract string WidgetTitle
        {
            get; set;
        }
    }
}
