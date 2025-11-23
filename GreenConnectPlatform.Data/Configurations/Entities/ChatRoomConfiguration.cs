using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasKey(e => e.ChatRoomId);
        builder.Property(e => e.ChatRoomId).ValueGeneratedNever();

        builder.HasOne(d => d.Transaction)
            .WithOne(p => p.ChatRoom)
            .HasForeignKey<ChatRoom>(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}