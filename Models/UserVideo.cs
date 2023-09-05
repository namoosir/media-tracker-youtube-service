using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    public class UserVideo
    {
        [Key]
        public int DataId { get; set; }

        public int UserId { get; set; }

        public string VideoId { get; set; }

        public string ChannelName { get; set; }

        public double WatchTime { get; set; }

        public string Genre { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
