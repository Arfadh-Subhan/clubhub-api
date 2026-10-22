using ClubHub.Data;
using ClubHub.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IClubMemberService, ClubMemberService>();
builder.Services.AddScoped<IClubRequestService, ClubRequestService>();
builder.Services.AddScoped<IClubPostService, ClubPostService>();
builder.Services.AddScoped<IClubAnnouncementService, ClubAnnouncementService>();
builder.Services.AddScoped<IClubMeetingService, ClubMeetingService>();
builder.Services.AddScoped<IClubGalleryService, ClubGalleryService>();
builder.Services.AddScoped<IGalleryPhotoService, GalleryPhotoService>();
builder.Services.AddScoped<IClubProductService, ClubProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRsvpService, EventRsvpService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ISiteReportService, SiteReportService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IIcbtAdminService, IcbtAdminService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<ILikeService, LikeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();