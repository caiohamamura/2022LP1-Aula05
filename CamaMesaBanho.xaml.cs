using MauiApp1.ViewModel;

namespace MauiApp1;

public partial class CamaMesaBanho : ContentPage
{
	public CamaMesaBanho(MyViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}