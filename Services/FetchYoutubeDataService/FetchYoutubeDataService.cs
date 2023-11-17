using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Utils.Youtube;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public class FetchYoutubeDataService : IFetchYoutubeDataService
{
    public FetchYoutubeDataService() { }

    public async Task<ServiceResponse<(List<Resource> items, string etag)>> FetchExternalPlaylists(
        string etag,
        YoutubeAPIClient client
    )
    {
        var serviceResponse = new ServiceResponse<(List<Resource> items, string etag)>();
        try
        {
            List<Resource> playlistsExternal = new();
            var playlistsResponse = await client.GetMyPlaylists(etag);
            if (playlistsResponse == null)
                throw new Exception("Playlists 304");

            var playlistsExternalEtag = playlistsResponse.etag;
            playlistsExternal.AddRange(playlistsResponse.items);

            while (playlistsResponse.nextPageToken != null)
            {
                playlistsResponse = await client.GetMyPlaylists(playlistsResponse.nextPageToken);
                playlistsExternal.AddRange(playlistsResponse.items);
            }

            serviceResponse.Data = (playlistsExternal, playlistsExternalEtag);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Resource>>> FetchExternalPlaylistVideos(
        YoutubeAPIClient client,
        string playlistId
    )
    {
        try
        {
            List<Resource> playlistVideosExternal = new();
            var playlistItemsResponse = await client.GetMyPlaylistItems(playlistId);
            if (playlistItemsResponse == null)
                return ServiceResponse<List<Resource>>.Build(playlistVideosExternal, true, null);
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

    public async Task<ServiceResponse<List<Resource>>> FetchChannels(
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
            if (channelIdsBlock.Count != 0)
            {
                var getChannelsResponse = await client.GetChannels(channelIdsBlock);
                channelsToUpdateResource.AddRange(getChannelsResponse.items);
            }
        }

        return ServiceResponse<List<Resource>>.Build(channelsToUpdateResource, true, null);
    }

    public async Task<
        ServiceResponse<(List<Resource> items, string etag)>
    > FetchExternalLikedVideos(YoutubeAPIClient client)
    {
        var serviceResponse = new ServiceResponse<(List<Resource>, string)>();

        try
        {
            var likedVideosExternal = new List<Resource>();
            var likedVideosResponse = await client.GetRatedVideos((YoutubeAPIClient.Rating)0);
            var likedVideosEtag = likedVideosResponse.etag;
            Console.WriteLine("\n\n\n\n\n\n\n" + likedVideosEtag);

            likedVideosExternal.AddRange(likedVideosResponse.items);

            while (likedVideosResponse.nextPageToken != null)
            {
                likedVideosResponse = await client.GetRatedVideos(
                    (YoutubeAPIClient.Rating)0,
                    likedVideosResponse.nextPageToken
                );
                likedVideosExternal.AddRange(likedVideosResponse.items);
            }

            serviceResponse.Data = (likedVideosExternal, likedVideosEtag);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<
        ServiceResponse<(List<Resource> items, string etag)>
    > FetchExternalDislikedVideos(YoutubeAPIClient client)
    {
        var serviceResponse = new ServiceResponse<(List<Resource> items, string etag)>();

        try
        {
            var dislikedVideosExternal = new List<Resource>();
            var dislikedVideosResponse = await client.GetRatedVideos((YoutubeAPIClient.Rating)1);
            var dislikedVideosEtag = dislikedVideosResponse.etag;

            Console.WriteLine("\n\n\n\n\n\n\n" + dislikedVideosEtag);

            dislikedVideosExternal.AddRange(dislikedVideosResponse.items);

            while (dislikedVideosResponse.nextPageToken != null)
            {
                dislikedVideosResponse = await client.GetRatedVideos(
                    (YoutubeAPIClient.Rating)1,
                    dislikedVideosResponse.nextPageToken
                );
                dislikedVideosExternal.AddRange(dislikedVideosResponse.items);
            }

            serviceResponse.Data = (dislikedVideosExternal, dislikedVideosEtag);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}
