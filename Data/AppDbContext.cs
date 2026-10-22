using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<ClubMember> ClubMembers => Set<ClubMember>();
    public DbSet<ClubRequest> ClubRequests => Set<ClubRequest>();
    public DbSet<ClubPost> ClubPosts => Set<ClubPost>();
    public DbSet<ClubAnnouncement> ClubAnnouncements => Set<ClubAnnouncement>();
    public DbSet<ClubMeeting> ClubMeetings => Set<ClubMeeting>();
    public DbSet<ClubGallery> ClubGalleries => Set<ClubGallery>();
    public DbSet<GalleryPhoto> GalleryPhotos => Set<GalleryPhoto>();
    public DbSet<ClubProduct> ClubProducts => Set<ClubProduct>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventRsvp> EventRsvps => Set<EventRsvp>();
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SiteReport> SiteReports => Set<SiteReport>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.StudentId)
            .IsUnique()
            .HasFilter("[StudentId] IS NOT NULL");

        modelBuilder.Entity<User>()
            .Property(user => user.StudentId)
            .HasMaxLength(20);

        modelBuilder.Entity<User>()
            .Property(user => user.FullName)
            .HasMaxLength(100);

        modelBuilder.Entity<User>()
            .Property(user => user.Email)
            .HasMaxLength(100);

        modelBuilder.Entity<User>()
            .Property(user => user.PasswordHash)
            .HasMaxLength(255);

        modelBuilder.Entity<User>()
            .Property(user => user.PhoneNumber)
            .HasMaxLength(15);

        modelBuilder.Entity<User>()
            .Property(user => user.Semester)
            .HasMaxLength(20);

        modelBuilder.Entity<User>()
            .Property(user => user.Role)
            .HasMaxLength(20);

        modelBuilder.Entity<Club>()
            .HasIndex(club => club.Name)
            .IsUnique();

        modelBuilder.Entity<Club>()
            .Property(club => club.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Club>()
            .Property(club => club.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<Club>()
            .Property(club => club.Category)
            .HasMaxLength(50);

        modelBuilder.Entity<Club>()
            .Property(club => club.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<Club>()
            .Property(club => club.RejectionReason)
            .HasMaxLength(500);

        modelBuilder.Entity<ClubMember>()
            .HasIndex(clubMember => new { clubMember.ClubId, clubMember.UserId })
            .IsUnique();

        modelBuilder.Entity<ClubMember>()
            .Property(clubMember => clubMember.ClubRole)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubMember>()
            .Property(clubMember => clubMember.Position)
            .HasMaxLength(50);

        modelBuilder.Entity<ClubMember>()
            .Property(clubMember => clubMember.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubRequest>()
            .Property(clubRequest => clubRequest.ProposedName)
            .HasMaxLength(100);

        modelBuilder.Entity<ClubRequest>()
            .Property(clubRequest => clubRequest.Category)
            .HasMaxLength(50);

        modelBuilder.Entity<ClubRequest>()
            .Property(clubRequest => clubRequest.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<ClubRequest>()
            .Property(clubRequest => clubRequest.Purpose)
            .HasMaxLength(1000);

        modelBuilder.Entity<ClubRequest>()
            .Property(clubRequest => clubRequest.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubRequest>()
            .Property(clubRequest => clubRequest.RejectionReason)
            .HasMaxLength(500);

        modelBuilder.Entity<ClubPost>()
            .Property(clubPost => clubPost.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<ClubPost>()
            .Property(clubPost => clubPost.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<ClubPost>()
            .Property(clubPost => clubPost.Category)
            .HasMaxLength(50);

        modelBuilder.Entity<ClubPost>()
            .Property(clubPost => clubPost.Links)
            .HasMaxLength(1000);

        modelBuilder.Entity<ClubPost>()
            .Property(clubPost => clubPost.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubPost>()
            .Property(clubPost => clubPost.RejectionReason)
            .HasMaxLength(500);

        modelBuilder.Entity<ClubAnnouncement>()
            .Property(clubAnnouncement => clubAnnouncement.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<ClubAnnouncement>()
            .Property(clubAnnouncement => clubAnnouncement.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<ClubAnnouncement>()
            .Property(clubAnnouncement => clubAnnouncement.Links)
            .HasMaxLength(1000);

        modelBuilder.Entity<ClubAnnouncement>()
            .Property(clubAnnouncement => clubAnnouncement.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubAnnouncement>()
            .Property(clubAnnouncement => clubAnnouncement.RejectionReason)
            .HasMaxLength(500);

        modelBuilder.Entity<ClubMeeting>()
            .Property(clubMeeting => clubMeeting.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<ClubMeeting>()
            .Property(clubMeeting => clubMeeting.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<ClubMeeting>()
            .Property(clubMeeting => clubMeeting.Location)
            .HasMaxLength(200);

        modelBuilder.Entity<ClubMeeting>()
            .Property(clubMeeting => clubMeeting.MeetingTime)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubMeeting>()
            .Property(clubMeeting => clubMeeting.Links)
            .HasMaxLength(1000);

        modelBuilder.Entity<ClubGallery>()
            .Property(clubGallery => clubGallery.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<ClubGallery>()
            .Property(clubGallery => clubGallery.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<GalleryPhoto>()
            .Property(galleryPhoto => galleryPhoto.Caption)
            .HasMaxLength(200);

        modelBuilder.Entity<ClubProduct>()
            .Property(clubProduct => clubProduct.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<ClubProduct>()
            .Property(clubProduct => clubProduct.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<ClubProduct>()
            .Property(clubProduct => clubProduct.Category)
            .HasMaxLength(50);

        modelBuilder.Entity<ClubProduct>()
            .Property(clubProduct => clubProduct.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ClubProduct>()
            .Property(clubProduct => clubProduct.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<ClubProduct>()
            .Property(clubProduct => clubProduct.RejectionReason)
            .HasMaxLength(500);

        modelBuilder.Entity<Order>()
            .Property(order => order.StudentId)
            .HasMaxLength(20);

        modelBuilder.Entity<Order>()
            .Property(order => order.FullName)
            .HasMaxLength(100);

        modelBuilder.Entity<Order>()
            .Property(order => order.PhoneNumber)
            .HasMaxLength(15);

        modelBuilder.Entity<Order>()
            .Property(order => order.Email)
            .HasMaxLength(100);

        modelBuilder.Entity<Order>()
            .Property(order => order.TotalPrice)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Order>()
            .Property(order => order.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.Name)
            .HasMaxLength(200);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.EventTime)
            .HasMaxLength(20);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.Venue)
            .HasMaxLength(200);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.Links)
            .HasMaxLength(1000);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.EstimatedBudget)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<Event>()
            .Property(eventItem => eventItem.RejectionReason)
            .HasMaxLength(500);

        modelBuilder.Entity<EventRsvp>()
            .HasIndex(eventRsvp => new { eventRsvp.EventId, eventRsvp.UserId })
            .IsUnique();

        modelBuilder.Entity<Reminder>()
            .HasIndex(reminder => new { reminder.EventId, reminder.UserId })
            .IsUnique();

        modelBuilder.Entity<Reminder>()
            .Property(reminder => reminder.Message)
            .HasMaxLength(200);

        modelBuilder.Entity<Notification>()
            .Property(notification => notification.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<Notification>()
            .Property(notification => notification.Message)
            .HasMaxLength(500);

        modelBuilder.Entity<Notification>()
            .Property(notification => notification.Type)
            .HasMaxLength(50);

        modelBuilder.Entity<SiteReport>()
            .Property(siteReport => siteReport.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<SiteReport>()
            .Property(siteReport => siteReport.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<SiteReport>()
            .Property(siteReport => siteReport.Status)
            .HasMaxLength(20);

        modelBuilder.Entity<ActivityLog>()
            .Property(activityLog => activityLog.Type)
            .HasMaxLength(20);

        modelBuilder.Entity<ActivityLog>()
            .Property(activityLog => activityLog.Message)
            .HasMaxLength(500);

        modelBuilder.Entity<ChatMessage>()
            .Property(chatMessage => chatMessage.Message)
            .HasMaxLength(1000);

        modelBuilder.Entity<Like>()
            .HasIndex(like => new { like.EventId, like.UserId })
            .IsUnique();
    }
}