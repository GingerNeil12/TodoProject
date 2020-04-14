using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoProject.Models;

namespace TodoProject.Data.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Note");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(x => x.CreatedAutomatically)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CreatedOn")
                .HasColumnType("Date")
                .HasDefaultValueSql("GetDate()")
                .IsRequired();

            builder.Property(x => x.TodoItemId)
                .HasColumnName("TodoItemId")
                .IsRequired();

            builder.HasOne(x => x.TodoItem)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
