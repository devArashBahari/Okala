namespace Okala.Api.Common;

public record AddReviewRequest(short Score, string ReviewTitle, string Comment, bool Recommended);