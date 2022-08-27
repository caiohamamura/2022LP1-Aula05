using MauiApp1.ViewModel;

namespace MauiApp1;

public partial class Jeans : ContentPage
{
	public Jeans(MyViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}