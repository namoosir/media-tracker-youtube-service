namespace MediaTrackerYoutubeService.GraphQL.UserVideos
{
    public record AddUserVideoInput(
        int UserId,
        string VideoId,
        string ChannelName,
        double WatchTime,
        string Genre,
        DateTime TimeStamp
    );
}
