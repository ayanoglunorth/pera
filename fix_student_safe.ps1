
# Safe UTF-8 fix script for student pages
# Fixes: wrong auth paths, duplicate DOMContentLoaded, missing popup functions, Turkish chars
# Uses [System.IO.File]::ReadAllText with explicit UTF8 to prevent encoding corruption

$baseUrl = 'http://localhost:5268'

$popupFunctions = @'

        async function loadMessagesPopup(token) {
            const container = document.getElementById("messagesPopupContent");
            try {
                const res = await fetch(`${baseUrl}/api/Messages/inbox`, { headers: { "Authorization": `Bearer ${token}` } });
                if (res.ok) {
                    const data = await res.json();
                    if (!data.length) { container.innerHTML = '<div class="empty-state"><p>Henüz mesaj yok</p></div>'; return; }
                    container.innerHTML = data.slice(0, 5).map(m => `<div class="popup-item" onclick="window.location.href='Messages.html'"><div class="popup-avatar">${m.fullName.charAt(0)}</div><div class="popup-text"><div class="popup-title">${m.fullName}</div><div class="popup-desc">${(m.lastMessage || '').substring(0, 40)}...</div></div></div>`).join('') + '<div class="popup-footer" style="padding:8px 16px;border-top:1px solid var(--border-color);"><a href="Messages.html" class="btn-ghost btn-sm">Tüm mesajları gör</a></div>';
                }
            } catch { container.innerHTML = '<div class="empty-state"><p>Yüklenemedi</p></div>'; }
        }
        async function loadNotificationsPopup(token) {
            const container = document.getElementById("notifPopupContent");
            const badge = document.getElementById("notifBadge");
            try {
                const res = await fetch(`${baseUrl}/api/Notifications`, { headers: { "Authorization": `Bearer ${token}` } });
                if (res.ok) {
                    const data = await res.json();
                    const unread = data.filter(n => !n.isRead).length;
                    if (badge) { badge.style.display = unread > 0 ? 'flex' : 'none'; badge.textContent = unread; }
                    if (!data.length) { container.innerHTML = '<div class="empty-state"><p>Henüz bildirim yok</p></div>'; return; }
                    container.innerHTML = data.slice(0, 5).map(n => `<div class="popup-item" onclick="window.location.href='Notifications.html'"><div class="popup-avatar"><i class="fa-solid fa-bell"></i></div><div class="popup-text"><div class="popup-title">${n.title}</div><div class="popup-desc">${(n.description || '').substring(0, 40)}...</div></div></div>`).join('') + '<div class="popup-footer" style="padding:8px 16px;border-top:1px solid var(--border-color);"><a href="Notifications.html" class="btn-ghost btn-sm">Tüm bildirimleri gör</a></div>';
                }
            } catch { container.innerHTML = '<div class="empty-state"><p>Yüklenemedi</p></div>'; }
        }
'@

# The duplicate DOMContentLoaded block to remove (same pattern in all files)
$duplicatePattern = @'
        document.addEventListener\("DOMContentLoaded", async \(\) => \{
            const token = localStorage\.getItem\("userToken"\);
            if \(!token\) \{ window\.location\.href = "(?:\.\./auth/)?Login\.html"; return; \}
            const user = parseJwt\(token\);
            if \(!user\) \{ (?:logout|cikisYap)\(\); return; \}

            const role = user\.role.*?;
            const userName = .*?"Kullan[ıi]c[ıi]".*?;
            const firstLetter = userName\.charAt\(0\)\.toUpperCase\(\);

            // Sidebar
            document\.getElementById\("sidebarAvatar"\)\.textContent = firstLetter;
            document\.getElementById\("sidebarName"\)\.textContent = userName;

            // Header
            const headerAvatar = document\.getElementById\("headerAvatar"\);
            if \(headerAvatar\) headerAvatar\.textContent = firstLetter;

            // Load data
            (?:await (?:fetchExams|loadExams|sinavlariYukle|karneYukle)\(token\);
            (?:renderCalendar\(\);
            )?)?loadMessagesPopup\(token\);
            loadNotificationsPopup\(token\);
        \}\);
'@

$files = @(
    "E:\Projects\Pera\Frontend\student\Calendar.html",
    "E:\Projects\Pera\Frontend\student\Exams.html",
    "E:\Projects\Pera\Frontend\student\Goals.html",
    "E:\Projects\Pera\Frontend\student\Grades.html",
    "E:\Projects\Pera\Frontend\student\Messages.html",
    "E:\Projects\Pera\Frontend\student\Notifications.html"
)

foreach ($f in $files) {
    $name = Split-Path $f -Leaf
    $content = [System.IO.File]::ReadAllText($f, [System.Text.Encoding]::UTF8)
    $original = $content
    $changes = @()

    # 1. Fix wrong auth redirect paths
    if ($content -match 'href = "Login\.html"') {
        $content = $content -replace 'href = "Login\.html"', 'href = "../auth/Login.html"'
        $changes += "auth_path"
    }

    # 2. Remove duplicate DOMContentLoaded (using regex for flexibility)
    $dupPattern = '(?s)        document\.addEventListener\("DOMContentLoaded", async \(\) => \{[^{]+?// Sidebar\r?\n            document\.getElementById\("sidebarAvatar"\)\.textContent = firstLetter;\r?\n            document\.getElementById\("sidebarName"\)\.textContent = userName;\r?\n\r?\n            // Header\r?\n            const headerAvatar = document\.getElementById\("headerAvatar"\);\r?\n            if \(headerAvatar\) headerAvatar\.textContent = firstLetter;\r?\n\r?\n            // Load data\r?\n            .*?loadNotificationsPopup\(token\);\r?\n        \}\);'
    if ($content -match $dupPattern) {
        $content = $content -replace $dupPattern, "        // (Duplicate DOMContentLoaded removed - merged into first listener above)"
        $changes += "dup_dom"
    }

    # 3. Add popup functions if missing
    if (($content -match "loadMessagesPopup") -and ($content -notmatch "async function loadMessagesPopup")) {
        $content = $content -replace "    </script>", "$popupFunctions    </script>"
        $changes += "popup_fn"
    }

    # 4. Fix "Kullanici" -> "Kullanıcı"  
    if ($content -match '"Kullanici"') {
        $content = $content -replace '"Kullanici"', '"Kullanıcı"'
        $changes += "kullanici"
    }

    if ($content -ne $original) {
        [System.IO.File]::WriteAllText($f, $content, [System.Text.Encoding]::UTF8)
        Write-Host "FIXED $name`: $($changes -join ', ')"
    } else {
        Write-Host "UNCHANGED: $name"
    }
}
Write-Host "`nDone."
