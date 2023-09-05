using HotChocolate.Subscriptions;
using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.GraphQL.UserVideos;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddUserVideoPayload> AddUserVideoAsync(
            AddUserVideoInput input,
            [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken
        )
        {
            var userVideo = new UserVideo
            {
                UserId = input.UserId,
                VideoId = input.VideoId,
                ChannelName = input.ChannelName,
                WatchTime = input.WatchTime,
                Genre = input.Genre,
                TimeStamp = input.TimeStamp
            };

            context.UserVideos.Add(userVideo);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(
                nameof(Subscription.OnUserVideoAdded),
                userVideo,
                cancellationToken
            );

            return new AddUserVideoPayload(userVideo);
        }

        //TODO: CONSIDER DOING UPDATE AND DELETE, FOLLOWS SAME TEMPLATE
    }
}
