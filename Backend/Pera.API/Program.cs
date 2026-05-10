using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.OpenApi.Models;
using Pera.Business.Abstract;   // Interface'ler i�in
using Pera.Business.Concrete;   // Class'lar i�in
using Pera.DataAccess;
using Pera.DataAccess.Abstract;
using Pera.DataAccess.Concrete;

using Pera.Entity.Entities;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Repository Kayd�: "Veritaban� i�lerini bu s�n�f yapar"

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();


// Auth Servisi Kayd�
builder.Services.AddScoped<IAuthService, AuthService>();

// Goal ve Notification Servisleri
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUploadedResultRepository, UploadedResultRepository>();
builder.Services.AddScoped<IUploadedResultService, UploadedResultService>();

// Bu sat�r, DbContext'e o arad��� "options" parametresini g�nderir.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddIdentity<AppUser, IdentityRole>() // <--- BURASI �OK �NEML�: IdentityRole EKLEND�
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
{
    options.AddPolicy("IzinVerilenler", policy =>
    {
        policy.AllowAnyOrigin()   // �imdilik herkese izin ver (Test i�in)
              .AllowAnyHeader()   // Her t�rl� ba�l��� kabul et
              .AllowAnyMethod();  // GET, POST, PUT, DELETE hepsine izin ver
    });
});

// --- JWT DO�RULAMA AYARLARI ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Appsettings.json'daki de�erlerle AYNI olmal�
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]))
    };
});

builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pera API", Version = "v1" });

    // Kilit simgesini ve Token giri�ini aktif eden ayar
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. �rnek: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("IzinVerilenler");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

await SeedDataAsync(app);

app.Run();

async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await context.Database.EnsureCreatedAsync();

    if (!await roleManager.RoleExistsAsync("Teacher"))
        await roleManager.CreateAsync(new IdentityRole("Teacher"));
    if (!await roleManager.RoleExistsAsync("Student"))
        await roleManager.CreateAsync(new IdentityRole("Student"));

    var teachers = new[]
    {
        ("Ayşe", "Yılmaz", "ayse.yilmaz@pera.com"),
        ("Mehmet", "Çelik", "mehmet.celik@pera.com"),
        ("Fatma", "Kara", "fatma.kara@pera.com"),
        ("Ahmet", "Demir", "ahmet.demir@pera.com"),
        ("Zeynep", "Şahin", "zeynep.sahin@pera.com")
    };

    var students = new[]
    {
        ("Emre", "Arslan", "emre.arslan@pera.com"),
        ("Buse", "Yıldırım", "buse.yildirim@pera.com"),
        ("Can", "Polat", "can.polat@pera.com"),
        ("Elif", "Aydın", "elif.aydin@pera.com"),
        ("Mert", "Korkmaz", "mert.korkmaz@pera.com")
    };

    foreach (var (first, last, email) in teachers)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new AppUser { UserName = email, Email = email, FirstName = first, LastName = last };
            var result = await userManager.CreateAsync(user, "Pera2024!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Teacher");
        }
    }

    foreach (var (first, last, email) in students)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new AppUser { UserName = email, Email = email, FirstName = first, LastName = last };
            var result = await userManager.CreateAsync(user, "Pera2024!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Student");
        }
    }

    var examTypes = new[] { "TYT", "AYT", "Deneme" };
    var examNames = new[] { "345 Yayınları", "Bilgi Sarmal", "Endemik", "Kartek", "Sınav Bozum", "Okyanus", "Açı", "Şenol Hoca" };
    var random = new Random();

    foreach (var teacher in teachers)
    {
        var tUser = await userManager.FindByEmailAsync(teacher.Item3);
        if (tUser == null) continue;

        for (int i = 0; i < 3; i++)
        {
            var exam = new Exam
            {
                Name = $"{examNames[random.Next(examNames.Length)]} {examTypes[random.Next(examTypes.Length)]}",
                Date = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                Type = examTypes[random.Next(examTypes.Length)]
            };
            context.Exams.Add(exam);
        }
    }
    await context.SaveChangesAsync();

    var exams = context.Exams.ToList();
    foreach (var student in students)
    {
        var sUser = await userManager.FindByEmailAsync(student.Item3);
        if (sUser == null) continue;

        for (int i = 0; i < 4; i++)
        {
            if (exams.Count == 0) break;
            var exam = exams[random.Next(exams.Count)];

            int turkish = random.Next(15, 35);
            int turkishW = random.Next(5, 15);
            int math = random.Next(15, 35);
            int mathW = random.Next(5, 15);
            int science = random.Next(12, 30);
            int scienceW = random.Next(5, 12);
            int history = random.Next(12, 28);
            int historyW = random.Next(5, 12);
            int religion = random.Next(10, 25);
            int religionW = random.Next(3, 10);
            int english = random.Next(8, 20);
            int englishW = random.Next(3, 10);

            var result = new ExamResult
            {
                ExamId = exam.Id,
                StudentId = sUser.Id,
                TurkishCorrect = turkish,
                TurkishWrong = turkishW,
                TurkishNet = turkish - (turkishW * 0.25m),
                MathCorrect = math,
                MathWrong = mathW,
                MathNet = math - (mathW * 0.25m),
                ScienceCorrect = science,
                ScienceWrong = scienceW,
                ScienceNet = science - (scienceW * 0.25m),
                HistoryCorrect = history,
                HistoryWrong = historyW,
                HistoryNet = history - (historyW * 0.25m),
                ReligionCorrect = religion,
                ReligionWrong = religionW,
                ReligionNet = religion - (religionW * 0.25m),
                EnglishCorrect = english,
                EnglishWrong = englishW,
                EnglishNet = english - (englishW * 0.25m),
                TotalNet = 0,
                Score = (turkish + math + science + history + religion + english) * 2.5m + 100
            };
            result.TotalNet = result.TurkishNet + result.MathNet + result.ScienceNet + result.HistoryNet + result.ReligionNet + result.EnglishNet;
            context.ExamResults.Add(result);
        }

        var goals = new List<(string Title, string Desc, string Type, int Current, int Target, DateTime? Due)>
        {
            ("TYT Net Hedefi", "Bu yıl TYT'den 300 nete ulaşmak istiyorum.", "Academic", 250, 300, DateTime.UtcNow.AddMonths(6)),
            ("Günlük Soru Çözümü", "Her gün en az 50 soru çözmek.", "Daily", 15, 50, null),
            ("Deneme Sınavı", "Bu ay en az 5 deneme sınavı bitirmek.", "Exam", 0, 5, DateTime.UtcNow.AddMonths(1))
        };

        var teacherUser = await userManager.FindByEmailAsync(teachers[0].Item3);
        foreach (var g in goals)
        {
            var goal = new Goal
            {
                StudentId = sUser.Id,
                TeacherId = teacherUser?.Id,
                Title = g.Title,
                Description = g.Desc,
                Type = g.Type,
                CurrentValue = g.Current,
                TargetValue = g.Target,
                DueDate = g.Due,
                IsCompleted = g.Current >= g.Target,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(5, 30))
            };
            if (goal.IsCompleted)
                goal.CompletedAt = DateTime.UtcNow;
            context.Goals.Add(goal);
        }
    }

    await context.SaveChangesAsync();
}
