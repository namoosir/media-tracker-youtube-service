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
        List<string> channelIds
    )
    {
        var serviceResponse = new ServiceResponse<List<Resource>>();

        int blockSize = 50;
        List<List<string>> channelIdsBlocks = Enumerable
            .Range(0, (channelIds.Count + blockSize - 1) / blockSize)
            .Select(i => channelIds.Skip(i * blockSize).Take(blockSize).ToList())
            .ToList();

        try
        {
            List<Resource> channelsResource = new List<Resource>();

            foreach (List<string> channelIdsBlock in channelIdsBlocks)
            {
                if (channelIdsBlock.Count != 0)
                {
                    var getChannelsResponse = await client.GetChannels(channelIdsBlock);
                    channelsResource.AddRange(getChannelsResponse.items);
                }
            }
            serviceResponse.Data = channelsResource;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Resource>>> FetchVideos(
        YoutubeAPIClient client,
        List<string> videoIds
    )
    {
        var serviceResponse = new ServiceResponse<List<Resource>>();

        int blockSize = 50;
        List<List<string>> videoIdsToUpdateBlocks = Enumerable
            .Range(0, (videoIds.Count + blockSize - 1) / blockSize)
            .Select(i => videoIds.Skip(i * blockSize).Take(blockSize).ToList())
            .ToList();

        try
        {
            var result = new List<Resource>();

            foreach (List<string> videoIdsBlock in videoIdsToUpdateBlocks)
            {
                if (videoIdsBlock.Count != 0)
                {
                    var getVideosResponse = await client.GetVideos(videoIdsBlock);
                    result.AddRange(getVideosResponse.items);
                }
            }

            serviceResponse.Data = result;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<(List<Resource> items, string etag)>> FetchSubscriptions(
        YoutubeAPIClient client,
        string internalEtag
    )
    {
        try
        {
            List<Resource> SubscriptionsExternal = new();
            var SubscriptionsResponse = await client.GetSubscriptions();
            var etag = SubscriptionsResponse.etag;
            if (etag == internalEtag)
                return ServiceResponse<(List<Resource> items, string etag)>.Build(
                    (SubscriptionsExternal, etag),
                    true,
                    null
                );

            SubscriptionsExternal.AddRange(SubscriptionsResponse.items);

            while (SubscriptionsResponse.nextPageToken != null)
            {
                SubscriptionsResponse = await client.GetMyPlaylistItems(
                    SubscriptionsResponse.nextPageToken
                );
                SubscriptionsExternal.AddRange(SubscriptionsResponse.items);
            }

            return ServiceResponse<(List<Resource> items, string etag)>.Build(
                (SubscriptionsExternal, etag),
                true,
                null
            );
        }
        catch (Exception e)
        {
            return ServiceResponse<(List<Resource> items, string etag)>.Build(
                (null, null),
                false,
                e.ToString()
            );
        }
    }

    public async Task<
        ServiceResponse<(List<Resource> items, string etag)>
    > FetchExternalRatedVideos(
        YoutubeAPIClient client,
        YoutubeAPIClient.Rating rating,
        string internalEtag
    )
    {
        var serviceResponse = new ServiceResponse<(List<Resource>, string)>();

        try
        {
            var ratedVideosExternal = new List<Resource>();
            var ratedVideosResponse = await client.GetRatedVideos(rating);
            var etag = ratedVideosResponse.etag;

            if (etag == internalEtag)
            {
                serviceResponse.Data = (ratedVideosExternal, etag);
                return serviceResponse;
            }

            ratedVideosExternal.AddRange(ratedVideosResponse.items);
            while (ratedVideosResponse.nextPageToken != null)
            {
                ratedVideosResponse = await client.GetRatedVideos(
                    rating,
                    ratedVideosResponse.nextPageToken
                );
                ratedVideosExternal.AddRange(ratedVideosResponse.items);
            }

            serviceResponse.Data = (ratedVideosExternal, etag);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    // public async Task<
    //     ServiceResponse<(List<Resource> items, string etag)>
    // > FetchExternalLikedVideos(YoutubeAPIClient client)
    // {
    //     var serviceResponse = new ServiceResponse<(List<Resource>, string)>();

    //     try
    //     {
    //         var likedVideosExternal = new List<Resource>();
    //         var likedVideosResponse = await client.GetRatedVideos((YoutubeAPIClient.Rating)0);
    //         var likedVideosEtag = likedVideosResponse.etag;

    //         likedVideosExternal.AddRange(likedVideosResponse.items);

    //         while (likedVideosResponse.nextPageToken != null)
    //         {
    //             likedVideosResponse = await client.GetRatedVideos(
    //                 (YoutubeAPIClient.Rating)0,
    //                 likedVideosResponse.nextPageToken
    //             );
    //             likedVideosExternal.AddRange(likedVideosResponse.items);
    //         }

    //         serviceResponse.Data = (likedVideosExternal, likedVideosEtag);
    //     }
    //     catch (Exception e)
    //     {
    //         serviceResponse.Success = false;
    //         serviceResponse.Message = e.Message;
    //     }

    //     return serviceResponse;
    // }

    // public async Task<
    //     ServiceResponse<(List<Resource> items, string etag)>
    // > FetchExternalDislikedVideos(YoutubeAPIClient client)
    // {
    //     var serviceResponse = new ServiceResponse<(List<Resource> items, string etag)>();

    //     try
    //     {
    //         var dislikedVideosExternal = new List<Resource>();
    //         var dislikedVideosResponse = await client.GetRatedVideos((YoutubeAPIClient.Rating)1);
    //         var dislikedVideosEtag = dislikedVideosResponse.etag;


    //         dislikedVideosExternal.AddRange(dislikedVideosResponse.items);

    //         while (dislikedVideosResponse.nextPageToken != null)
    //         {
    //             dislikedVideosResponse = await client.GetRatedVideos(
    //                 (YoutubeAPIClient.Rating)1,
    //                 dislikedVideosResponse.nextPageToken
    //             );
    //             dislikedVideosExternal.AddRange(dislikedVideosResponse.items);
    //         }

    //         serviceResponse.Data = (dislikedVideosExternal, dislikedVideosEtag);
    //     }
    //     catch (Exception e)
    //     {
    //         serviceResponse.Success = false;
    //         serviceResponse.Message = e.Message;
    //     }

    //     return serviceResponse;
    // }
}
