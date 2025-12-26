using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(e => e.MessageId);
        builder.Property(e => e.MessageId).ValueGeneratedNever();
        builder.Property(e => e.Timestamp).HasDefaultValueSql("now()");

        builder.HasIndex(e => new { e.ChatRoomId, e.Timestamp }).IsDescending(false, true);

        builder.HasOne(d => d.ChatRoom)
            .WithMany(p => p.Messages)
            .HasForeignKey(d => d.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Sender)
            .WithMany(p => p.Messages)
            .HasForeignKey(d => d.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}