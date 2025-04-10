using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public sealed class BaseIdentityRoleClaimEntityConfiguration
    : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable(IdentityRoleClaimEntity.Metadata.TableName);
    }
}
