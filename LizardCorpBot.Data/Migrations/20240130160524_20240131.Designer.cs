﻿// <auto-generated />
using System;
using LizardCorpBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LizardCorpBot.Data.Migrations
{
    [DbContext(typeof(LizardBotDbContext))]
    [Migration("20240130160524_20240131")]
    partial class _20240131
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LizardCorpBot.Data.Model.Guild", b =>
                {
                    b.Property<decimal>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("GuildId");

                    b.ToTable("guild");
                });

            modelBuilder.Entity("LizardCorpBot.Data.Model.GuildUser", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("user_id");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("GuildId", "UserId");

                    b.ToTable("guild_user");
                });

            modelBuilder.Entity("LizardCorpBot.Data.Model.MinecraftUser", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("IsPlaying")
                        .HasColumnType("boolean")
                        .HasColumnName("is_playing");

                    b.Property<DateTime?>("LastJoined")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_joined");

                    b.Property<long>("PlayTime")
                        .HasColumnType("bigint")
                        .HasColumnName("playtime");

                    b.Property<decimal?>("UserId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("user_id");

                    b.HasKey("Name");

                    b.ToTable("minecraft_user");
                });

            modelBuilder.Entity("LizardCorpBot.Data.Model.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Author")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("author");

                    b.Property<DateTime?>("CompleteTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("complete_time");

                    b.Property<decimal>("ConfirmerId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("confirmer");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_time");

                    b.Property<decimal>("Guild")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("message_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<decimal[]>("TaskHolder")
                        .IsRequired()
                        .HasColumnType("numeric(20,0)[]")
                        .HasColumnName("taskholders");

                    b.Property<DateTime?>("TimeLimit")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time_limit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("todo");
                });

            modelBuilder.Entity("LizardCorpBot.Data.Model.TodoChannel", b =>
                {
                    b.Property<decimal>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("channel_id");

                    b.HasKey("GuildId");

                    b.ToTable("todo_channel");
                });
#pragma warning restore 612, 618
        }
    }
}
