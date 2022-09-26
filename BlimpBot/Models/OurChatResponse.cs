using System;

namespace BlimpBot.Models
{
    public record OurChatResponse
    {
        public string Text { get; init; } = string.Empty;
        public string PhotoUrl { get; init; }
        public bool IsPhotoMessage { get; init; }
    }
}