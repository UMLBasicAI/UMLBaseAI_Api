using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.ToTable(MessageEntity.Metadata.TableName);
        builder.HasKey(entity => entity.Id);

        builder
            .Property(m => m.Content)
            .HasColumnName(MessageEntity.Metadata.Properties.Content.ColumnName)
            .HasMaxLength(MessageEntity.Metadata.Properties.Content.MaxLength)
            .IsRequired(MessageEntity.Metadata.Properties.Content.IsNotNull);

        builder
            .Property(m => m.MessageType)
            .HasColumnName(MessageEntity.Metadata.Properties.MessageType.ColumnName)
            .HasMaxLength(MessageEntity.Metadata.Properties.MessageType.MaxLength)
            .IsRequired(MessageEntity.Metadata.Properties.MessageType.IsNotNull);

        builder
            .Property(m => m.SentAt)
            .HasColumnName(MessageEntity.Metadata.Properties.SentAt.ColumnName)
            .IsRequired(MessageEntity.Metadata.Properties.SentAt.IsNotNull);
    }
}
