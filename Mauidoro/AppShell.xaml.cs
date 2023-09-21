namespace Mauidoro;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(DetailTaskPage), typeof(DetailTaskPage));
	}
}
