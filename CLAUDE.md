# Pera — Eğitim Yönetim Sistemi

## Proje Hakkında

Pera, öğretmen ve öğrencilerin aynı panelde çalışabildiği, sınav takibi ve sonuç analizi yapabilen bir eğitim yönetim platformudur. Rol bazlı yönlendirme ile iki ayrı panel sunar.

**Teknoloji Yığını:**
- Backend: ASP.NET Core 8, PostgreSQL (Npgsql), Entity Framework, JWT Authentication
- Frontend: Vanilla HTML/CSS/JS, Font Awesome 6, Inter font ailesi
- Veritabanı: PostgreSQL — `Host=localhost;Port=5432;Database=Pera;Username=postgres;Password=ayanoglu`

**Klasör Yapısı:**
```
E:/Projects/Pera/
├── Backend/
│   ├── Pera.API/            → Web API, Controllers, Program.cs
│   ├── Pera.Business/      → Services (ExamService, GoalService, vb.)
│   ├── Pera.DataAccess/    → Repositories, DbContext, Migrations
│   ├── Pera.DTO/           → Data Transfer Objects
│   ├── Pera.Entity/        → Entity classes (AppUser, Exam, Goal, vb.)
│   └── Pera.sln
├── Frontend/
│   ├── auth/               → Login.html, SignUp.html
│   ├── teacher/            → Öğretmen paneli (Dashboard, Exams, Grades, Goals, Calendar, Messages, Notifications, Profile, Settings, ExamDefine, ExamAdd, ExamEntry, WrittenGradeEntry)
│   ├── student/            → Öğrenci paneli (Dashboard, Exams, Grades, Goals, Calendar, Messages, Notifications, Profile, Settings)
│   ├── shared/             → (kullanılmıyor, boş)
│   ├── pera-layout.css     → Global CSS
│   └── DESIGN.md           → Tasarım sistemi dokümantasyonu
```

---

## Komutlar

### Backend
```bash
cd E:/Projects/Pera/Backend/Pera.API
dotnet run          # API çalıştır → http://localhost:5268
dotnet build        # Derle
dotnet ef database update  # Migration uygula
```

### Frontend
```bash
# API zaten çalışıyor durumda, sadece tarayıcıda aç
# Login: http://localhost:5268 (veya frontend dosyasını tarayıcıda aç)
```

---

## Mimari Kurallar

### Backend Katmanları
```
Controller → Service (Business) → Repository (DataAccess) → Entity
```
- **Controllers:** HTTP isteklerini karşılar, `[Authorize]` ile JWT doğrulama zorunlu
- **Services:** İş mantığı, transaction yönetimi
- **Repositories:** Veritabanı CRUD, DbContext üzerinden PostgreSQL'e erişim
- **DTOs:** API request/response modelleri

### Veritabanı
- PostgreSQL kullanılır, SQL Server'a **kesinlikle izin verilmez**
- `AppDbContext.cs`'de `UseNpgsql` kullanılmalı, `UseSqlServer` kesinlikle yasak
- Connection string: `appsettings.json` → `ConnectionStrings:DefaultConnection`

### Roller ve Yetkilendirme
- İki rol: `Teacher` ve `Student`
- JWT'de hem `ClaimTypes.Role` hem de `role` claim'i kullanılır
- Giriş yapılınca rol bazlı yönlendirme: Teacher → `teacher/Dashboard.html`, Student → `student/Dashboard.html`

### Frontend API Entegrasyonu
- Tüm API çağrıları `baseUrl = "http://localhost:5268"` üzerinden yapılır
- JWT token `localStorage.userToken`'da saklanır
- Her isteğe `Authorization: Bearer {token}` header'ı eklenir

---

## Kod Konvansiyonları

### Genel
- Tüm frontend dosyaları UTF-8, Türkçe içerik
- CSS değişkenleri (`:root`) DESIGN.md'de tanımlı — mavi (`--primary-blue`), sarı (`--yellow-main`), zemin (`--primary-bg`) standart olarak kullanılır
- Border-radius hiyerarşisi: kartlar 16px, ara container'lar 12px, butonlar 8px
- `i.pravatar.cc` ve `unsplash` harici görsel kullanımı **yasak** — avatar baş harf dairesi ile gösterilir

### Frontend JavaScript
- `parseJwt(token)` → JWT'yi decode eder, user object döner
- Popup açılır: `togglePopup(id)`, kapatılır: dışarı tıklanınca otomatik kapanır
- Header avatar her zaman baş harf dairesi (`<div class="header-avatar-circle">A</div>`)
- Sahte/hardcoded veri kullanımı **yasak** — tüm veriler API'den gelmeli

### Backend C#
- Null reference uyarıları tolere edilir (nullable reference types), ancak gerçek null check yapılmalı
- Controller'da `User.FindFirst(ClaimTypes.NameIdentifier)?.Value` ile userId alınır
- Rol kontrolü: `role == "Teacher"` string karşılaştırması

---

## Mevcut Durum

### Tamamlanmış
- ✅ Rol bazlı panel ayrımı (teacher/ ve student/)
- ✅ JWT authentication ile giriş/çıkış
- ✅ API entegrasyonu (Dashboard, Messages, Notifications, Goals, Exams)
- ✅ Header popup'larında dinamik veri gösterimi
- ✅ Sahte veri temizliği (pravatar resimleri, hardcoded isimler, placeholder alert'ler)
- ✅ PostgreSQL'e tam geçiş

### Yapılacak / Eksik
- ⚠️ `newGoal()` fonksiyonu placeholder — backend endpoint'i mevcut ama form UI'ı yok
- ⚠️ `ResultUpload.html` sidebar'da link var ama dosya yok (nav'dan kaldırıldı)
- ⚠️ Rozet/hedef gibi gamification verileri backend'de yok — şimdilik profil sayfasında kaldırıldı

---

## Dosya Özgü Notlar

### Backend kritik dosyalar
- `Pera.API/Program.cs` — DI kayıtları, JWT ayarları, CORS
- `Pera.DataAccess/AppDbContext.cs` — PostgreSQL bağlantısı, Entity konfigürasyonu
- `Pera.Business/Concrete/AuthService.cs` — JWT token üretimi, rol atama

### Frontend kritik dosyalar
- `teacher/Dashboard.html` — Popup sistemi ve API entegrasyonu için referans
- `auth/Login.html` — Rol bazlı yönlendirme mantığı
- `pera-layout.css` — Global tasarım sistemi

---

## CLI Kısayolları

- `dotnet build` → Backend derle
- `dotnet run` → API başlat (5268 port)
- Backend klasörüne gir: `cd E:/Projects/Pera/Backend/Pera.API`

---

*Bu dosya projeyi anlamak için gereken temel bağlamı sağlar. Detaylı tasarım kuralları için `Frontend/DESIGN.md`'ye bak.*