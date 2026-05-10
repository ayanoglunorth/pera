
$files = @(
    "E:\Projects\Pera\Frontend\student\Calendar.html",
    "E:\Projects\Pera\Frontend\student\Exams.html",
    "E:\Projects\Pera\Frontend\student\Goals.html",
    "E:\Projects\Pera\Frontend\student\Grades.html",
    "E:\Projects\Pera\Frontend\student\Messages.html",
    "E:\Projects\Pera\Frontend\student\Notifications.html"
)

$correctNavMenu = @'
        <ul class="nav-menu">
            <li class="nav-item"><a href="Dashboard.html" class="nav-link"><i class="fa-solid fa-house"></i> Panel</a></li>
            <li class="nav-item"><a href="Exams.html" class="nav-link"><i class="fa-regular fa-file-lines"></i> Sınavlarım</a></li>
            <li class="nav-item"><a href="Grades.html" class="nav-link"><i class="fa-solid fa-graduation-cap"></i> Notlarım</a></li>
            <li class="nav-item"><a href="Goals.html" class="nav-link"><i class="fa-regular fa-folder"></i> Hedeflerim</a></li>
            <li class="nav-item"><a href="Calendar.html" class="nav-link"><i class="fa-regular fa-calendar"></i> Takvim</a></li>
            <li class="nav-item"><a href="Notifications.html" class="nav-link"><i class="fa-regular fa-bell"></i> Bildirimler</a></li>
            <li class="nav-item"><a href="Messages.html" class="nav-link"><i class="fa-regular fa-envelope"></i> Mesajlar</a></li>
            <li class="nav-item"><a href="Settings.html" class="nav-link"><i class="fa-solid fa-gear"></i> Ayarlar</a></li>
        </ul>
'@

# We need to find the existing <ul class="nav-menu"> block and replace it.
# The existing one might span multiple lines.

foreach ($f in $files) {
    $content = [System.IO.File]::ReadAllText($f, [System.Text.Encoding]::UTF8)
    $name = Split-Path $f -Leaf
    $modified = $false

    # 1. Fix CSS path
    if ($content -match 'href="pera-layout.css"') {
        $content = $content -replace 'href="pera-layout.css"', 'href="../pera-layout.css"'
        $modified = $true
    }

    # 2. Fix the sidebar nav-menu
    # We match from <ul class="nav-menu"> to </ul>
    $navPattern = '(?s)<ul class="nav-menu">.*?</ul>'
    if ($content -match $navPattern) {
        $oldNav = [regex]::Match($content, $navPattern).Value
        # If it contains "Öğrenciler", it's the wrong teacher menu
        if ($oldNav -match 'Öğrenciler') {
            $content = $content -replace $navPattern, $correctNavMenu
            $modified = $true
        }
    }

    # 3. Apply active class properly based on file name
    $pageMap = @{
        "Dashboard.html" = "Panel"
        "Exams.html" = "Sınavlarım"
        "Grades.html" = "Notlarım"
        "Goals.html" = "Hedeflerim"
        "Calendar.html" = "Takvim"
        "Notifications.html" = "Bildirimler"
        "Messages.html" = "Mesajlar"
        "Settings.html" = "Ayarlar"
    }
    
    $activeItem = $pageMap[$name]
    if ($activeItem) {
        # First remove any existing 'active' class from nav-item
        $content = $content -replace '<li class="nav-item active">', '<li class="nav-item">'
        # Then add 'active' to the correct one
        $targetLi = '<li class="nav-item"><a href="' + $name + '"'
        $replacementLi = '<li class="nav-item active"><a href="' + $name + '"'
        $content = $content.Replace($targetLi, $replacementLi)
    }

    if ($modified) {
        [System.IO.File]::WriteAllText($f, $content, [System.Text.Encoding]::UTF8)
        Write-Host "Fixed: $name"
    } else {
        Write-Host "No changes needed: $name"
    }
}
