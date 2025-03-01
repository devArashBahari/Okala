namespace Okala.Api.Common;

public record AddReviewResponse(short Score, string ReviewTitle, string Comment, bool Recommended);