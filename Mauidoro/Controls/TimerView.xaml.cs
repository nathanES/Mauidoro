using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;

namespace Mauidoro.Controls;

public partial class TimerView : ContentView, INotifyPropertyChanged
{
    #region Title
    public static readonly BindableProperty TimerTitleProperty = BindableProperty.Create(nameof(TimerTitle), typeof(string), typeof(TimerView), string.Empty);
    
    public string TimerTitle
    {
        get => (string)GetValue(TimerView.TimerTitleProperty);
        set => SetValue(TimerView.TimerTitleProperty, value);
    }
    #endregion

    public TimerView()
    {
        InitializeComponent();
        // StartTimerCommand = new Command(async () => await StartTimerAsync()); //Mettre une methode asynchrone
    }

    private int i;
    public int I
    {
        get => i;
        set
        {
            if (i == value)
                return;
            i = value;
            OnPropertyChanged(nameof(I));
        }
    }


    public System.Timers.Timer timer;
    public ICommand StartTimerCommand => new Command(StartTimer);
    private  void StartTimer()
    {
        SetTimer();
    }

    private void SetTimer()
    {
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = true;
        timer.Enabled = true;
    }
    
    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        I++;
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            e.SignalTime);
    }
    

    public ICommand StopTimerCommand => new Command(StopTimer);

    private  void StopTimer()
    {
        timer.Stop();
        timer.Dispose();
        I = 0;
    }

    
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    // private  async Task StartTimerAsync()
    // {
    //     I++;
    // }
}