using Base.DataBaseAndIdentity.Common;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class HistoryEntityConfiguration : IEntityTypeConfiguration<HistoryEntity>
{
    public void Configure(EntityTypeBuilder<HistoryEntity> builder)
    {
        builder.ToTable(HistoryEntity.Metadata.TableName);
        builder.HasKey(entity => entity.Id);

        builder
            .Property(entity => entity.Action)
            .HasColumnName(HistoryEntity.Metadata.Properties.Action.ColumnName)
            .HasColumnType(Constant.DatabaseType.TEXT)
            .IsRequired(HistoryEntity.Metadata.Properties.Action.IsNotNull);

        builder
            .Property(entity => entity.PlantUMLCode)
            .HasColumnName(HistoryEntity.Metadata.Properties.PlantUMLCode.ColumnName)
            .HasColumnType(Constant.DatabaseType.TEXT)
            .IsRequired(HistoryEntity.Metadata.Properties.PlantUMLCode.IsNotNull);

        //foreign key to IdentityUserEntity
        builder
            .Property(entity => entity.UserId)
            .HasColumnName(HistoryEntity.Metadata.Properties.UserId.ColumnName)
            .IsRequired(HistoryEntity.Metadata.Properties.UserId.IsNotNull);
        builder
            .HasOne(history => history.IdentityUser)
            .WithMany(user => user.Histories)
            .HasForeignKey(entity => entity.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
