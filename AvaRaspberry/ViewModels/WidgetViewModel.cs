using AvaRaspberry.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public abstract class WidgetViewModel : ViewModelBase, IWidgetViewModel
    {

        private string _widgetTitle;

        public virtual string WidgetTitle
        {
            get => _widgetTitle;
            set => this.RaiseAndSetIfChanged(ref _widgetTitle, value);
        }

       
    }
}