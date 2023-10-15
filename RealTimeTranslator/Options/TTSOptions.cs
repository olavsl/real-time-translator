namespace RealTimeTranslator.Options
{
    public class TTSOptions
    {
        public required string SubscriptionKey { get; set; }
        public required string Region { get; set; }
        public required string VoiceName { get; set; }
        public required string Language { get; set; }
    }
}