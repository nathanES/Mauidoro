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
            OnPropertyChanged(nameof(IsPauseMode));
            OnPropertyChanged(nameof(IsFocusMode));
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
            OnPropertyChanged(nameof(CanBreakTimer));
        }
    }
    public ICommand StartTimerCommand => new Command(SetAndStartTimer, ()=> CanStartTimer);
    public void SetAndStartTimer() => this.OnTimerStarted();
    
    public bool CanBreakTimer
    {
        get => !CanStartTimer;
    }
    public ICommand BreakTimerCommand => new Command(BreakTimer, () => CanBreakTimer);
    public void BreakTimer() => this.OnTimerBreaked();


    private void TimerStarted(object? sender, EventArgs e)
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
        
        CanStartTimer = false;
    }
    private void TimerBreaked(object? sender, EventArgs e)
    {
        _timer.Stop();
        CanStartTimer = true;
    }
    
    private void TimerFinished(object? sender, EventArgs e)
    {
        if (IsPauseMode)
            OnTimerPauseFinished();
        else
            OnTimerFocusFinished();
    }

    private void TimerFocusFinished(object? sender, EventArgs e)
    {
        _iTravailSessionDone++;
        IsFocusMode = !IsFocusMode;
        TimerSessionPomodoro = GetTimerTimeSpanSession();
    }
    private void TimerPauseFinished(object? sender, EventArgs e)
    {
        _iPauseSessionDone++;
        _timer.Stop();
        _timer.Dispose();
        CanStartTimer = true;
        
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
    
    public TimerView()
    {        
        InitializeComponent();
        IsFocusMode = true;
        TimerSessionPomodoro = GetTimerTimeSpanSession();
        TimerStartedEvent += TimerStarted;
        TimerBreakedEvent += TimerBreaked;
        TimerFinishedEvent += TimerFinished;
        TimerFocusFinishedEvent += TimerFocusFinished;
        TimerPauseFinishedEvent += TimerPauseFinished;
        CanStartTimer = true;

        // StartTimerCommand = new Command(async () => await StartTimerAsync()); //Mettre une methode asynchrone
    }

    // private  async Task StartTimerAsync()
    // {
    //     I++;
    // }
    #region Events
    public static event EventHandler? TimerStartedEvent;
    protected virtual void OnTimerStarted()
        => TimerStartedEvent?.Invoke(this, EventArgs.Empty);

    public static event EventHandler? TimerBreakedEvent;

    protected virtual void OnTimerBreaked()
        => TimerBreakedEvent?.Invoke(this, EventArgs.Empty);

    public static event EventHandler? TimerFinishedEvent;
    protected virtual void OnTimerFinished()//EventArgs e)
        => TimerFinishedEvent?.Invoke(this, EventArgs.Empty);

    public static event EventHandler? TimerFocusFinishedEvent;
    protected virtual void OnTimerFocusFinished()//EventArgs e)
        => TimerFocusFinishedEvent?.Invoke(this, EventArgs.Empty);
    
    public static event EventHandler? TimerPauseFinishedEvent;
    protected virtual void OnTimerPauseFinished()//EventArgs e)
        => TimerPauseFinishedEvent?.Invoke(this, EventArgs.Empty);
    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
}