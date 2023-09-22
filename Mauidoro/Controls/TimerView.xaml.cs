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

    private int _iPauseSessionDone { get; set; } = 0; //Si iPause est %4 == 0 alors on est dans une longue Pause
    private int _iTravailSessionDone { get; set; } = 0; //Si iPause est %4 == 0 alors on est dans une longue Pause

    private int _iSessionDone { get => _iPauseSessionDone + _iTravailSessionDone;}
    private System.Timers.Timer _timer;
    
    private TimeSpan _timerSessionPomodoro;
    public TimeSpan TimerSessionPomodoro
    {
        get => _timerSessionPomodoro;
        private set
        {
            if (_timerSessionPomodoro == value)
                return;
            _timerSessionPomodoro = value;

            if (_timerSessionPomodoro == new TimeSpan(0, 0, 0))
                OnTimerFinished();
            OnPropertyChanged(nameof(TimerSessionPomodoro));
        }
    }

    private bool _isFocusMode;
    public bool IsFocusMode
    {
        get => _isFocusMode;
        set
        {
            if(_isFocusMode == value)
                return;
            _isFocusMode = value;
            OnPropertyChanged(nameof(IsFocusMode));
            OnPropertyChanged(nameof(IsPauseMode));
        }
        
    }
    public bool IsPauseMode => !IsFocusMode;

    private bool _canStartTimer;
    public bool CanStartTimer
    {
        get => _canStartTimer;
        set
        {
            if (_canStartTimer == value)
                return;
            _canStartTimer = value;
            OnPropertyChanged(nameof(CanStartTimer));
        }
    }
    public ICommand StartTimerCommand => new Command(SetAndStartTimer, ()=> CanStartTimer);
    public void SetAndStartTimer() => this.OnTimerStarted();

    private bool _canBreakTimer;
    public bool CanBreakTimer
    {
        get => _canBreakTimer;
        set
        {
            if (_canBreakTimer == value)
                return;
            _canBreakTimer = value;
            OnPropertyChanged(nameof(CanBreakTimer));
        }
    }
    public ICommand BreakTimerCommand => new Command(BreakTimer, () => CanBreakTimer);
    public void BreakTimer() => this.OnTimerBreaked();

    private bool _canStopTimer;

    public bool CanStopTimer
    {
        get => _canStopTimer;
        set
        {
            if (_canStopTimer == value)
                return;
            _canStopTimer = value;
            OnPropertyChanged(nameof(CanStopTimer));
        }
    }
    public ICommand StopTimerCommand => new Command(StopTimer, () => CanStopTimer);
    public void StopTimer() =>this.OnTimerStopped();


    #region Events
    public event EventHandler? TimerStartedEvent;
    protected virtual void OnTimerStarted()
        => TimerStartedEvent?.Invoke(this, EventArgs.Empty);

    public event EventHandler? TimerBreakedEvent;

    protected virtual void OnTimerBreaked()
        => TimerBreakedEvent?.Invoke(this, EventArgs.Empty);

    public event EventHandler? TimerStoppedEvent;

    protected virtual void OnTimerStopped() 
        => TimerStoppedEvent?.Invoke(this, EventArgs.Empty);

    public event EventHandler? TimerFinishedEvent;
    protected virtual void OnTimerFinished()//EventArgs e)
        => TimerFinishedEvent?.Invoke(this, EventArgs.Empty);

    #endregion
    
    private void TimerStarted(object? sender, EventArgs e)
    {
        Console.WriteLine("TimerStarted");   
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
        
        CanStartTimer = false;
        CanBreakTimer = true;
        CanStopTimer = true;
    }
    private void TimerBreaked(object? sender, EventArgs e)
    {
        _timer.Stop();
        CanStartTimer = true;
        CanBreakTimer = false;
        CanStopTimer = true;
    }
    private void TimerStopped(object? sender, EventArgs e)
    {        
        _timer.Stop();
        _timer.Dispose();
        SetUpNextSession();
        CanStartTimer = true;
        CanBreakTimer = false;
        CanStopTimer = false;
    }
    
    private void TimerFinished(object? sender, EventArgs e)
    {
        Console.WriteLine("TimerFinished");
        if (IsPauseMode)
        {
            _timer.Stop();
            _timer.Dispose();
            CanStartTimer = true;
            CanBreakTimer = false;
            CanStopTimer = false;
        };
        SetUpNextSession();
    }

    private void SetUpNextSession()
    {
        if (IsPauseMode)
            _iPauseSessionDone++;
        else
            _iTravailSessionDone++;
        IsFocusMode = !IsFocusMode;
        TimerSessionPomodoro = GetTimerTimeSpanSession();
    }
    
    
    private void OnTimedEvent(Object source, ElapsedEventArgs e) 
        =>TimerSessionPomodoro = TimerSessionPomodoro.Subtract(new TimeSpan(0, 0, 1));
    private TimeSpan GetTimerTimeSpanSession()
    {
        if (IsFocusMode)
            return new TimeSpan(0, 0, 5);//Pour les tests //remplacer par 25minutes
        else if (_iTravailSessionDone % 4 == 0 && _iPauseSessionDone !=0)
            return new TimeSpan(0, 0, 3);//remplacer par 15minutes
        else
            return new TimeSpan(0, 0, 2); //remplacer par 5minutes
    }
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
    
    public TimerView()
    {        
        InitializeComponent();
        IsFocusMode = true;
        TimerSessionPomodoro = GetTimerTimeSpanSession();
        TimerStartedEvent += TimerStarted;
        TimerBreakedEvent += TimerBreaked;
        TimerStoppedEvent += TimerStopped;
        TimerFinishedEvent += TimerFinished;
        CanStartTimer = true;
        CanStopTimer = false;
        CanBreakTimer = false;

        // StartTimerCommand = new Command(async () => await StartTimerAsync()); //Mettre une methode asynchrone
    }

    // private  async Task StartTimerAsync()
    // {
    //     I++;
    // }
}