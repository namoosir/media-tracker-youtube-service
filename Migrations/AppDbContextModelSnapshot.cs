﻿// <auto-generated />
using System;
using MediaTrackerYoutubeService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChannelUser", b =>
                {
                    b.Property<string>("SubscribedChannelsYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("UserSubscribersUserId")
                        .HasColumnType("int");

                    b.HasKey("SubscribedChannelsYoutubeId", "UserSubscribersUserId");

                    b.HasIndex("UserSubscribersUserId");

                    b.ToTable("ChannelUser", (string)null);
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Channel", b =>
                {
                    b.Property<string>("YoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ETag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Imported")
                        .HasColumnType("bit");

                    b.Property<long?>("SubscriberCount")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("VideoCount")
                        .HasColumnType("bigint");

                    b.Property<long?>("ViewCount")
                        .HasColumnType("bigint");

                    b.HasKey("YoutubeId");

                    b.ToTable("Channels", (string)null);
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Playlist", b =>
                {
                    b.Property<string>("YoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ETag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("YoutubeId");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists", (string)null);
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DislikedVideosEtag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LikedVideosEtag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaylistsEtag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubscriptionsEtag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Video", b =>
                {
                    b.Property<string>("YoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChannelYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("CommentCount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ETag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Imported")
                        .HasColumnType("bit");

                    b.Property<long?>("LikeCount")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ViewCount")
                        .HasColumnType("bigint");

                    b.HasKey("YoutubeId");

                    b.HasIndex("ChannelYoutubeId");

                    b.ToTable("Videos", (string)null);
                });

            modelBuilder.Entity("PlaylistVideo", b =>
                {
                    b.Property<string>("PlaylistYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VideosYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PlaylistYoutubeId", "VideosYoutubeId");

                    b.HasIndex("VideosYoutubeId");

                    b.ToTable("PlaylistVideo", (string)null);
                });

            modelBuilder.Entity("UserVideo", b =>
                {
                    b.Property<int>("LikedByUsersUserId")
                        .HasColumnType("int");

                    b.Property<string>("LikedVideosYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LikedByUsersUserId", "LikedVideosYoutubeId");

                    b.HasIndex("LikedVideosYoutubeId");

                    b.ToTable("UserVideo", (string)null);
                });

            modelBuilder.Entity("UserVideo1", b =>
                {
                    b.Property<int>("DislikedByUsersUserId")
                        .HasColumnType("int");

                    b.Property<string>("DislikedVideosYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DislikedByUsersUserId", "DislikedVideosYoutubeId");

                    b.HasIndex("DislikedVideosYoutubeId");

                    b.ToTable("UserVideo1", (string)null);
                });

            modelBuilder.Entity("ChannelUser", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.Channel", null)
                        .WithMany()
                        .HasForeignKey("SubscribedChannelsYoutubeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediaTrackerYoutubeService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserSubscribersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Playlist", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.User", "User")
                        .WithMany("VideoPlaylists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Video", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.Channel", "Channel")
                        .WithMany("Videos")
                        .HasForeignKey("ChannelYoutubeId");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("PlaylistVideo", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.Playlist", null)
                        .WithMany()
                        .HasForeignKey("PlaylistYoutubeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediaTrackerYoutubeService.Models.Video", null)
                        .WithMany()
                        .HasForeignKey("VideosYoutubeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserVideo", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("LikedByUsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediaTrackerYoutubeService.Models.Video", null)
                        .WithMany()
                        .HasForeignKey("LikedVideosYoutubeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserVideo1", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("DislikedByUsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediaTrackerYoutubeService.Models.Video", null)
                        .WithMany()
                        .HasForeignKey("DislikedVideosYoutubeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Channel", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.User", b =>
                {
                    b.Navigation("VideoPlaylists");
                });
#pragma warning restore 612, 618
        }
    }
}
