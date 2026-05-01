# Design System Specification: Pera Dashboard (The Warm & Gamified Hub)

## 1. Genel Bakış ve Yaratıcı Vizyon: "Sıcak ve Oyunlaştırılmış Merkez"
Pera Dashboard, öğrencilerin ve eğitmenlerin verilerini soğuk tablolar yerine samimi, modern ve motive edici bir arayüzle sunmayı hedefler. Yaratıcı vizyonumuz **"Sıcak ve Oyunlaştırılmış Merkez"**dir.

Tasarım; geniş boşluklar (gaps & paddings), yuvarlatılmış köşeler (border-radius) ve dikkat dağıtmayan yumuşak bir zemin üzerinde yükselen belirgin kartlardan oluşur. Kullanıcıyı boğmayan, odaklanmasını sağlayan bir yapıya sahiptir.

---

## 2. Renkler: CSS Değişkenleri ve Hiyerarşi
Renk paleti, temiz bir zemin üzerine inşa edilmiş mavi ve sarı vurgulardan oluşur. Kodda belirtilen CSS değişkenleri (`:root`) referans alınmalıdır.

### Ana Palet (Primary Palette)
- **Zemin (`--primary-bg`: `#f5f6fa`):** Tüm `body` arka planı.
- **Yüzey (`--white`: `#ffffff`):** Tüm `.card` ve `.sidebar` arka planları.
- **Koyu Metin (`--text-dark`: `#1e293b`):** Varsayılan metin rengi ve önemli vurgular.
- **Gri Metin (`--text-gray`: `#64748b`):** Alt başlıklar, menü linkleri, zaman damgaları ve ikonlar.
- **Birincil Mavi (`--primary-blue`: `#2563eb`):** Sayfa başlığı, ana butonlar ve XP rozetleri. *(Hover durumu: `--hover-blue`: `#1d4ed8`)*
- **Aksan Sarı (`--yellow-main`: `#facc15`):** Aktif menü öğeleri, butonlar, 3. sıra XP rozetleri ve sparkline grafikleri.
- **Açık Sarı (`--yellow-light`: `#fef08a`):** Liderlik tablosundaki skor arka planları.
- **Kenarlıklar (`--border-color`: `#e2e8f0`):** Arama çubuğu arka planı, sol menü ve liste ayırıcı çizgileri.

### İmza Geçişler (Gradients)
- **Geri Sayım Kartı:** `linear-gradient(135deg, #fde047 0%, #f97316 100%)`
- **Konu Performansı (Donut):** `conic-gradient(#3b82f6 0% 35%, #06b6d4 35% 60%, #f43f5e 60% 75%, #facc15 75% 100%)`

---

## 3. Tipografi: Inter ile Net Hiyerarşi
Sistem genelinde yalnızca **Inter** font ailesi kullanılır. Ağırlıklar `300, 400, 500, 600, 700, 800` arasından seçilerek güçlü bir tipografik kontrast yaratılır.

- **Dev Rakam (Countdown):** `42px / Font-Weight: 800` (Sadece LGS Geri Sayım sayısında)
- **Sayfa Başlığı (Page Title):** `28px / Font-Weight: 700` (`--primary-blue` renginde)
- **Metrik/Skor (Stat Value):** `28px / Font-Weight: 700` (`--text-dark` renginde)
- **Logo (Sidebar):** `24px / Font-Weight: 700`
- **Kart Başlıkları (Card Headers):** `16px / Font-Weight: 600` (Performans ve Liderlik kartları)
- **Menü Linkleri (Nav Links):** `16px (default) / Font-Weight: 500`
- **Gövde & Etiketler (Body/Labels):** `14px / Font-Weight: 600` (Alt başlıklar ve isimler)
- **Küçük Metinler (Muted Subtext):** `12px / Regular` (Tarihler, XP değerleri, alt açıklamalar)

---

## 4. Yükselti ve Sınırlar (Elevation & Borders)
Derinlik algısı, keskin çizgilerden ziyade yumuşak gölgeler ve açık renk border'lar ile sağlanır.

- **Kart Gölgeleri (`--card-shadow`):** `0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03)`
- **Ayrım Çizgileri:** Yalnızca Sidebar sağ kenarında ve Liderlik Listesi alt öğelerinde (`border-bottom`) `1px solid var(--border-color)` kullanılır. Kart içlerinde ayırıcı çizgi kullanılmaz.

---

## 5. Bileşenler: Köşe Radyüs (Border-Radius) Kuralları
Tasarımın "yumuşaklık" hissi, border-radius değerlerinin katı kurallara bağlanmasıyla elde edilmiştir.

- **16px (Büyük Konteynerler):** Tüm `.card` sınıfları.
- **12px (Orta Konteynerler):** Aktif menü bağlantıları (`.nav-link`), kullanıcı profil kutusu, skor listesi arka planları ve ikon kapsayıcıları.
- **8px (Küçük Araçlar):** Ana `.btn-primary` butonları ve arama çubuğu (`.search-bar`).
- **Tam Yuvarlak (50% veya dairesel):** Avatarlar, bildirim noktaları (`.badge`), grafik efsane noktaları (`.dot`).
- **Özel (20px):** Geri sayım kartındaki aksiyon butonu (daha kapsül formunda).

---

## 6. Yapılacaklar ve Yapılmayacaklar (Do's and Don'ts)

### Yapılacaklar (Do):
- **Do** CSS Grid kullanımını teşvik edin. Ana layout için `gap: 24px` ve kart içlerindeki alt gridler için `gap: 20px` boşluk (whitespace) kuralına sadık kalın.
- **Do** Liderlik tablosundaki ilk 3 dereceyi göstermek için avatarların boyutunu asimetrik yapın (2. olanı `65px`, diğerlerini `45px` tutarak ortadakini vurgulayın) ve çerçevelerini renklendirin.
- **Do** Aktif menü durumunu sadece metin rengiyle değil, `--yellow-main` arka planıyla belirginleştirin.

### Yapılmayacaklar (Don't):
- **Don't** Kart içindeki hiyerarşiyi ayırmak için yatay veya dikey gri çizgiler kullanmayın; hiyerarşiyi boşluk (`margin-bottom: 20px` vb.) ve tipografi ile çözün.
- **Don't** Arama kutusuna veya iç input alanlarına `border` atamayın. Bunun yerine sadece `--border-color` (#e2e8f0) ile arka plan rengi verin.
- **Don't** Gölgeleri (box-shadow) gereğinden fazla karartmayın. Şeffaflık seviyesi (opacity) `%5` ve `%3` değerlerini kesinlikle geçmemelidir.