using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class BaseIdentityRoleEntityConfiguration
    : IEntityTypeConfiguration<IdentityRoleEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleEntity> builder)
    {
        builder.ToTable(IdentityRoleEntity.Metadata.TableName);
    }
}
