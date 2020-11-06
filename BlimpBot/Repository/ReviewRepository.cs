using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlimpBot.Constants;
using BlimpBot.Database;
using BlimpBot.Database.Models;
using BlimpBot.ExtensionMethods;
using BlimpBot.Interfaces;

namespace BlimpBot.Repository
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        private readonly BlimpBotContext _context;

        public ReviewRepository(BlimpBotContext context)
        {
            Context = context;
            _context = context;
        }
        
        public string GetChatResponse(List<string> arguments)
        {
            Review review = null;
            if(arguments.Count == 0) review = GetReview();
            else if (arguments.Count == 1)
            {
                if (arguments[0].ToLower() == "recent") review = GetMostRecentReview();
                else review = GetReviewByCategory(ParseCategory(arguments[0]));
            }
            return ParseReviewIntoTelegramString(review);
        }

        private string ParseReviewIntoTelegramString(Review review)
        {
            if (review == null) return "No such review!";
            
            var emojiString = GetReviewEmojiString(review);
            
            return $"<b>{review.Title}</b>\n" +
                   $"{emojiString}\n" +
                   $"{review.ShortText}";
        }

        private string GetReviewEmojiString(Review review)
        {
            var rawString = string.IsNullOrEmpty(review.Emoji) ? "2B50" : review.Emoji;
            var emojiString = "";
            foreach(var codePoint in rawString.Split(' ')){
                var utf32 = int.Parse(codePoint, System.Globalization.NumberStyles.HexNumber);
                emojiString += char.ConvertFromUtf32(utf32);
            }
            var emojiCount = (int) Math.Floor(review.Rating / 2.0);
            emojiString = string.Concat(Enumerable.Repeat(emojiString, emojiCount));
            var isHalf = review.Rating % 2 != 0;
            if(isHalf)
                emojiString += char.ConvertFromUtf32(0x00BD);
            
            return emojiString;
        }

        private ReviewCategory ParseCategory(string categoryString)
        {
            return Enum.TryParse(categoryString, out ReviewCategory category) ? category : ReviewCategory.Any;
        }

        public Review GetReview()
        {
            return _context.Reviews.RandomRow();
        }

        public Review GetMostRecentReview()
        {
            return _context.Reviews.OrderByDescending(i => i.DateUpdated)
                           .FirstOrDefault();
        }

        public Review GetReviewByMinimumRating(int rating)
        {
            return _context.Reviews.Where(i=>i.Rating > rating).RandomRow();
        }
        public Review GetReviewByCategory(ReviewCategory category)
        {
            if (category == ReviewCategory.Any) return GetReview();
            return _context.Reviews.Where(i => i.ReviewCategory == category)
                                   .RandomRow();
        }

        public Review GetReviewByCategoryAndMinimumRating(ReviewCategory category, int rating)
        {
            if (category == ReviewCategory.Any) return GetReviewByMinimumRating(rating);
            return _context.Reviews.Where(i => i.ReviewCategory == category && i.Rating > rating)
                           .RandomRow();
        }

        public Review GetReviewById(int reviewId)
        {
            return _context.Reviews.Find(reviewId);
        }

    }
}
