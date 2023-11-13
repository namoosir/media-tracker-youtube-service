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
        Console.WriteLine("JKSDNJLKVC");
        Console.WriteLine(channelsToUpdate.Count);

        List<string> channelIdsToUpdate = channelsToUpdate
            .Select(channel => channel.YoutubeId)
            .ToList();

        for (int i = 0; i < channelIdsToUpdate.Count; i++)
        {
            Console.WriteLine("HOHO");
            Console.WriteLine(channelIdsToUpdate[i]);
        }

        int blockSize = 50;
        List<List<string>> channelIdsToUpdateBlocks = Enumerable
            .Range(0, (channelIdsToUpdate.Count + blockSize - 1) / blockSize)
            .Select(i => channelIdsToUpdate.Skip(i * blockSize).Take(blockSize).ToList())
            .ToList();

        Console.WriteLine("JSKFJLKHDSNJ\n\n\n\n");
        Console.WriteLine("Accessing elements of the 2D list:");
        for (int i = 0; i < channelIdsToUpdateBlocks.Count; i++)
        {
            for (int j = 0; j < channelIdsToUpdateBlocks[i].Count; j++)
            {
                Console.Write(channelIdsToUpdateBlocks[i][j] + " K ");
            }
            Console.WriteLine();
        }

        List<Resource> channelsToUpdateResource = new List<Resource>();
        foreach (List<string> channelIdsBlock in channelIdsToUpdateBlocks)
        {
            if (channelIdsBlock.Count != 0)
            {
                var getChannelsResponse = await client.GetChannels(channelIdsBlock);
                channelsToUpdateResource.AddRange(getChannelsResponse.items);
            }
        }

        return ServiceResponse<List<Resource>>.Build(channelsToUpdateResource, true, null);
    }
}
