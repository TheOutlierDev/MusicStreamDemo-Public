namespace MusicStreamDemo.API.Data.Entities
{
    public class OpenAiResponseEntity
    {
        public Choice[] Choices { get; set; }

        public class Choice
        {
            public Message Message { get; set; }
        }

        public class Message
        {
            public string Content { get; set; }
        }
    }
}
