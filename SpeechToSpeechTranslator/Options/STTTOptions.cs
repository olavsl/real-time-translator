namespace SpeechToSpeechTranslator.Options
{
    public class STTTOptions
    {
        public required string SubscriptionKey { get; set; }
        public required string Region { get; set; }
        public required string SourceLanguage { get; set; }
        public required string TargetLanguage { get; set; }
    }
}