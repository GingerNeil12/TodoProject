using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoProject.Models;

namespace TodoProject.Data.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.ToTable("TodoItem");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .HasMaxLength(2056)
                .IsRequired();

            builder.Property(x => x.HasStarted)
                .HasColumnName("HasStarted");

            builder.Property(x => x.StartedOn)
                .HasColumnName("StartedOn")
                .HasColumnType("Date");

            builder.Property(x => x.HasCompleted)
                .HasColumnName("HasCompleted");

            builder.Property(x => x.CompletedOn)
                .HasColumnName("CompletedOn")
                .HasColumnType("Date");

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CreatedOn")
                .HasColumnType("Date")
                .HasDefaultValueSql("GetDate()")
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.TodoItems)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.TodoItems)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Notes)
                .WithOne(x => x.TodoItem)
                .HasForeignKey(x => x.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
