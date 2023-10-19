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
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Channel", b =>
                {
                    b.Property<string>("YoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ETag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubscriberCount")
                        .HasColumnType("int");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("VideoCount")
                        .HasColumnType("int");

                    b.Property<int?>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("YoutubeId");

                    b.HasIndex("UserId");

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

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
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

                    b.Property<int?>("CommentCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ETag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LikeCount")
                        .HasColumnType("int");

                    b.Property<string>("PlaylistYoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("YoutubeId");

                    b.HasIndex("ChannelYoutubeId");

                    b.HasIndex("PlaylistYoutubeId");

                    b.ToTable("Videos", (string)null);
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Channel", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.User", null)
                        .WithMany("SubscribedChannels")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Playlist", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.User", null)
                        .WithMany("VideoPlaylists")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Video", b =>
                {
                    b.HasOne("MediaTrackerYoutubeService.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelYoutubeId");

                    b.HasOne("MediaTrackerYoutubeService.Models.Playlist", null)
                        .WithMany("Videos")
                        .HasForeignKey("PlaylistYoutubeId");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.Playlist", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("MediaTrackerYoutubeService.Models.User", b =>
                {
                    b.Navigation("SubscribedChannels");

                    b.Navigation("VideoPlaylists");
                });
#pragma warning restore 612, 618
        }
    }
}
