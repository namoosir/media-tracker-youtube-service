using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic]
        public UserVideo OnUserVideoAdded([EventMessage] UserVideo userVideo)
        {
            return userVideo;
        }
    }
}
