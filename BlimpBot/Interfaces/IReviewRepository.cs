using System.Collections.Generic;
using BlimpBot.Constants;
using BlimpBot.Database.Models;

namespace BlimpBot.Interfaces
{
    public interface IReviewRepository : IChatCommandRepository
    {
        Review GetReview();
        Review GetReviewByCategory(ReviewCategory category);

        Review GetReviewByCategoryAndMinimumRating(ReviewCategory category, int rating);

        Review GetReviewById(int reviewId);

    }
}
