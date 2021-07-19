namespace AppiPot
{
    public interface INotification
    {

        bool NeedsWatering { get; set; }

        void StartForegroundServiceCompat();
        
    }
    
    
}