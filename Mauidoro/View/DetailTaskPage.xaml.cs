using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mauidoro.ViewModel;

namespace Mauidoro;

public partial class DetailTaskPage : ContentPage
{
    public DetailTaskPage(DetailTaskViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}