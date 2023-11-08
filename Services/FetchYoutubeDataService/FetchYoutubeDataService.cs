using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Utils.Youtube;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public class FetchYoutubeDataService : IFetchYoutubeDataService
{
    public FetchYoutubeDataService() { }

    public async Task<ServiceResponse<List<Resource>>> fetchExternalPlaylists(
        YoutubeAPIClient client
    )
    {
        try
        {
            List<Resource> playlistsExternal = new();
            var playlistsResponse = await client.GetMyPlaylists();

            playlistsExternal.AddRange(playlistsResponse.items);

            while (playlistsResponse.nextPageToken != null)
            {
                playlistsResponse = await client.GetMyPlaylists(playlistsResponse.nextPageToken);
                playlistsExternal.AddRange(playlistsResponse.items);
            }

            return ServiceResponse<List<Resource>>.Build(playlistsExternal, true, null);
        }
        catch (Exception e)
        {
            return ServiceResponse<List<Resource>>.Build(null, false, e.ToString());
        }
    }

    public async Task<ServiceResponse<List<Resource>>> fetchExternalPlaylistVideos(
        YoutubeAPIClient client,
        string playlistId
    )
    {
        try
        {
            List<Resource> playlistVideosExternal = new();
            var playlistItemsResponse = await client.GetMyPlaylistItems(playlistId);

            playlistVideosExternal.AddRange(playlistItemsResponse.items);

            while (playlistItemsResponse.nextPageToken != null)
            {
                playlistItemsResponse = await client.GetMyPlaylistItems(
                    playlistId,
                    playlistItemsResponse.nextPageToken
                );
                playlistVideosExternal.AddRange(playlistItemsResponse.items);
            }

            return ServiceResponse<List<Resource>>.Build(playlistVideosExternal, true, null);
        }
        catch (Exception e)
        {
            return ServiceResponse<List<Resource>>.Build(null, false, e.ToString());
        }
    }

    public async Task<ServiceResponse<List<Resource>>> fetchChannels(
        YoutubeAPIClient client,
        List<Channel> channelsToUpdate
    )
    {
        List<string> channelIdsToUpdate = channelsToUpdate
            .Select(channel => channel.YoutubeId)
            .ToList();

        int blockSize = 50;
        List<List<string>> channelIdsToUpdateBlocks = Enumerable
            .Range(0, (channelIdsToUpdate.Count + blockSize - 1) / blockSize)
            .Select(i => channelIdsToUpdate.Skip(i * blockSize).Take(blockSize).ToList())
            .ToList();

        List<Resource> channelsToUpdateResource = new List<Resource>();
        foreach (List<string> channelIdsBlock in channelIdsToUpdateBlocks)
        {
            var getChannelsResponse = await client.GetChannels(channelIdsBlock);
            channelsToUpdateResource.AddRange(getChannelsResponse.items);
        }

        return ServiceResponse<List<Resource>>.Build(channelsToUpdateResource, true, null);
    }
}
