
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

foreach ($f in $files) {
    $lines = [System.IO.File]::ReadAllLines($f, [System.Text.Encoding]::UTF8)
    $name = Split-Path $f -Leaf
    
    $startIdx = -1
    $endIdx = -1
    
    for ($i = 0; $i -lt $lines.Length; $i++) {
        if ($lines[$i] -match '<ul class="nav-menu">') {
            $startIdx = $i
        }
        if ($startIdx -ne -1 -and $lines[$i] -match '</ul>') {
            $endIdx = $i
            break
        }
    }

    if ($startIdx -ne -1 -and $endIdx -ne -1) {
        $menuContent = $lines[$startIdx..$endIdx] -join "`n"
        
        # Only replace if it contains the teacher "Öğrenciler" menu or "Sınav Tanımla"
        if ($menuContent -match 'ExamDefine.html') {
            # Build new file content
            $newLines = @()
            if ($startIdx -gt 0) { $newLines += $lines[0..($startIdx - 1)] }
            $newLines += $correctNavMenu.Split("`n") | ForEach-Object { $_.TrimEnd("`r") }
            if ($endIdx -lt ($lines.Length - 1)) { $newLines += $lines[($endIdx + 1)..($lines.Length - 1)] }
            
            # Now set the active class for the current file
            $finalLines = @()
            foreach ($line in $newLines) {
                if ($line -match '<li class="nav-item">.*href="' + $name + '"') {
                    $finalLines += $line -replace '<li class="nav-item">', '<li class="nav-item active">'
                } else {
                    $finalLines += $line
                }
            }
            
            [System.IO.File]::WriteAllLines($f, $finalLines, [System.Text.Encoding]::UTF8)
            Write-Host "Fixed sidebar in: $name"
        } else {
            Write-Host "Sidebar already correct in: $name"
        }
    } else {
        Write-Host "Could not find nav-menu in: $name"
    }
}
