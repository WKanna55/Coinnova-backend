using Coinnova.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Chat> Chat { get; set; }

    public virtual DbSet<Comment> Comment { get; set; }

    public virtual DbSet<Community> Community { get; set; }

    public virtual DbSet<CommunityCategory> CommunityCategory { get; set; }

    public virtual DbSet<CommunityMember> CommunityMember { get; set; }

    public virtual DbSet<Event> Event { get; set; }

    public virtual DbSet<EventCategory> EventCategory { get; set; }

    public virtual DbSet<Institution> Institution { get; set; }

    public virtual DbSet<InstitutionEvent> InstitutionEvent { get; set; }

    public virtual DbSet<Message> Message { get; set; }

    public virtual DbSet<Notification> Notification { get; set; }

    public virtual DbSet<Post> Post { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-little-star-a4jjx428.us-east-1.aws.neon.tech;Database=Coinnova;Username=Coinnova_owner;Password=npg_N4howcgQ7aBb;Port=5432;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("comment_type", new[] { "Encontré un problema", "Sugerencia de mejora", "Tengo dudas", "Me gusta la idea" })
            .HasPostgresEnum("event_visibility", new[] { "public", "private" })
            .HasPostgresEnum("post_type", new[] { "Validación de prototipo", "Consulta", "Creación de grupo", "Generación de idea" })
            .HasPostgresEnum("user_role", new[] { "admin", "standard" });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_pkey");

            entity.ToTable("chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.IdUser1).HasColumnName("id_user1");
            entity.Property(e => e.IdUser2).HasColumnName("id_user2");

            entity.HasOne(d => d.IdUser1Navigation).WithMany(p => p.ChatIdUser1Navigation)
                .HasForeignKey(d => d.IdUser1)
                .HasConstraintName("chat_id_user1_fkey");

            entity.HasOne(d => d.IdUser2Navigation).WithMany(p => p.ChatIdUser2Navigation)
                .HasForeignKey(d => d.IdUser2)
                .HasConstraintName("chat_id_user2_fkey");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comment_pkey");

            entity.ToTable("comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.IdParentComment).HasColumnName("id_parent_comment");
            entity.Property(e => e.IdPost).HasColumnName("id_post");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.IdParentCommentNavigation).WithMany(p => p.InverseIdParentCommentNavigation)
                .HasForeignKey(d => d.IdParentComment)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comment_id_parent_comment_fkey");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Comment)
                .HasForeignKey(d => d.IdPost)
                .HasConstraintName("comment_id_post_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comment)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("comment_id_user_fkey");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("community_pkey");

            entity.ToTable("community");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdInstitution).HasColumnName("id_institution");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.IdInstitutionNavigation).WithMany(p => p.Community)
                .HasForeignKey(d => d.IdInstitution)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("community_id_institution_fkey");
        });

        modelBuilder.Entity<CommunityCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("community_category_pkey");

            entity.ToTable("community_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdCommunity).HasColumnName("id_community");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.CommunityCategory)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("community_category_id_category_fkey");

            entity.HasOne(d => d.IdCommunityNavigation).WithMany(p => p.CommunityCategory)
                .HasForeignKey(d => d.IdCommunity)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("community_category_id_community_fkey");
        });

        modelBuilder.Entity<CommunityMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("community_member_pkey");

            entity.ToTable("community_member");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCommunity).HasColumnName("id_community");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Joinedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("joinedat");

            entity.HasOne(d => d.IdCommunityNavigation).WithMany(p => p.CommunityMember)
                .HasForeignKey(d => d.IdCommunity)
                .HasConstraintName("community_member_id_community_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.CommunityMember)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("community_member_id_user_fkey");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Initialdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("initialdate");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Place)
                .HasMaxLength(255)
                .HasColumnName("place");
            entity.Property(e => e.Rulesurl).HasColumnName("rulesurl");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Event)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("event_createdby_fkey");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_category_pkey");

            entity.ToTable("event_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdEvent).HasColumnName("id_event");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.EventCategory)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_category_id_category_fkey");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.EventCategory)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_category_id_event_fkey");
        });

        modelBuilder.Entity<Institution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("institution_pkey");

            entity.ToTable("institution");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Domain)
                .HasMaxLength(255)
                .HasColumnName("domain");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<InstitutionEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("institution_event_pkey");

            entity.ToTable("institution_event");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEvent).HasColumnName("id_event");
            entity.Property(e => e.IdInstitution).HasColumnName("id_institution");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.InstitutionEvent)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("institution_event_id_event_fkey");

            entity.HasOne(d => d.IdInstitutionNavigation).WithMany(p => p.InstitutionEvent)
                .HasForeignKey(d => d.IdInstitution)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("institution_event_id_institution_fkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pkey");

            entity.ToTable("message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.IdChat).HasColumnName("id_chat");
            entity.Property(e => e.IdSender).HasColumnName("id_sender");

            entity.HasOne(d => d.IdChatNavigation).WithMany(p => p.Message)
                .HasForeignKey(d => d.IdChat)
                .HasConstraintName("message_id_chat_fkey");

            entity.HasOne(d => d.IdSenderNavigation).WithMany(p => p.Message)
                .HasForeignKey(d => d.IdSender)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("message_id_sender_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Entity)
                .HasMaxLength(50)
                .HasColumnName("entity");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.RefId).HasColumnName("ref_id");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Notification)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("notification_id_user_fkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("post_pkey");

            entity.ToTable("post");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.IdCommunity).HasColumnName("id_community");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.Textcontent).HasColumnName("textcontent");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.IdCommunityNavigation).WithMany(p => p.Post)
                .HasForeignKey(d => d.IdCommunity)
                .HasConstraintName("post_id_community_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Post)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("post_id_user_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.HasIndex(e => e.Email, "User_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Biography).HasColumnName("biography");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IdInstitution).HasColumnName("id_institution");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");

            entity.HasOne(d => d.IdInstitutionNavigation).WithMany(p => p.User)
                .HasForeignKey(d => d.IdInstitution)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("User_id_institution_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
