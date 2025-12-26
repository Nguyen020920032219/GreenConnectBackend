using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.HasKey(e => new { e.UserId, e.ChatRoomId });
        builder.Property(e => e.JoinedAt).HasDefaultValueSql("now()");

        builder.HasOne(d => d.ChatRoom)
            .WithMany(p => p.ChatParticipants)
            .HasForeignKey(d => d.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.User)
            .WithMany(p => p.ChatParticipants)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}