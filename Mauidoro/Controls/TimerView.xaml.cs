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
    private System.Timers.Timer timer;
    
    
    //Pomodoro :
    //1 Pomodoro : 25 minutes de travail + 5 minutes de pauses
    //Après le 4ième pomodoro la pause est de 15 minutes
    private int iPause = 0; //Si iPause est %4 == 0 alors on est dans une longue Pause

    private bool isFocusMode = true;
    public bool IsFocusMode
    {
        get => isFocusMode;
        set
        {
            if (isFocusMode == value)
                return;
            
            isFocusMode = value;
            if (!isFocusMode)
                iPause++;
            OnPropertyChanged(nameof(IsFocusMode));
        }
    }
    // private bool isPauseMode { get => !isFocusMode; }
    
    private TimeSpan timerSessionDoro;
    public TimeSpan TimerSessionDoro
    {
        get => timerSessionDoro;
        private set
        {
            if (timerSessionDoro == value)
                return;
            
            if (value == new TimeSpan(0, 0, 0))
            {
                IsFocusMode = !IsFocusMode;
                timerSessionDoro = GetTimerTimeSpanSession();
            }
            else
                timerSessionDoro = value;
            OnPropertyChanged(nameof(TimerSessionDoro));
        }
    }

    private TimeSpan GetTimerTimeSpanSession()
    {
        if (IsFocusMode)
            return new TimeSpan(0, 0, 5);//Pour les tests //remplacer par 25minutes
        else if (iPause % 4 == 0 && iPause !=0)
            return new TimeSpan(0, 0, 3);//remplacer par 15minutes
        else
            return new TimeSpan(0, 0, 2); //remplacer par 5minutes
    }

    public ICommand StartTimerCommand => new Command(StartTimer, ()=> CanStartTimer);
    private bool canStartTimer = true;
    public bool CanStartTimer
    {
        get => canStartTimer;
        set
        {
            if(canStartTimer == value)
                return;
            canStartTimer = value;
            OnPropertyChanged(nameof(CanStartTimer));
        }
    }
    private  void StartTimer()
    {
        SetTimer();
        CanStartTimer = false;
        CanStopTimer = true;
        CanBreakTimer = true;
    }

    private bool canStopTimer = false;
    public bool CanStopTimer
    {
        get => canStopTimer;
        set
        {
            if(canStopTimer == value)
                return;
            canStopTimer = value;
            OnPropertyChanged(nameof(CanStopTimer));
        }
    }

    public ICommand StopTimerCommand => new Command(StopTimer, () => CanStopTimer);
    
    private void StopTimer()
    {
        timer.Stop();
        timer.Dispose();
        
        timerSessionDoro = new TimeSpan(0, 0, 0);
        CanStopTimer = false;
        CanStartTimer = true;
        CanBreakTimer = false;
        
    }

    private bool canBreakTimer = false;
    public bool CanBreakTimer
    {
        get => canBreakTimer;
        set
        {
            if(canBreakTimer == value)
                return;
            canBreakTimer = value;
            OnPropertyChanged(nameof(CanBreakTimer));
        }
    }
    public ICommand BreakTimerCommand => new Command(BreakTimer, () => CanBreakTimer);
    private void BreakTimer()
    {
        timer.Stop();
        
        CanBreakTimer = false;
        CanStartTimer = true;
        CanStopTimer = true;
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
        TimerSessionDoro = TimerSessionDoro.Subtract(new TimeSpan(0, 0, 1));
        // Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
        //     e.SignalTime);
    }
    
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    
    public TimerView()
    {
        InitializeComponent();
        TimerSessionDoro = GetTimerTimeSpanSession();
        // StartTimerCommand = new Command(async () => await StartTimerAsync()); //Mettre une methode asynchrone
    }

    // private  async Task StartTimerAsync()
    // {
    //     I++;
    // }
}