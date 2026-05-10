
$cssToAppend = @"

/* Popup Styles added during standardization */
.popup-container { position: relative; }
.popup-container button { outline: none; border: none; background: none; }
.popup-panel { display: none; position: absolute; top: calc(100% + 8px); right: 0; width: 320px; background: var(--white); border-radius: 12px; box-shadow: 0 10px 40px rgba(0,0,0,0.12); border: 1px solid var(--border-color); z-index: 1000; }
.popup-panel.active { display: block; }
.popup-header { display: flex; justify-content: space-between; align-items: center; padding: 14px 16px; border-bottom: 1px solid var(--border-color); }
.popup-header h3 { font-size: 15px; font-weight: 600; margin: 0; }
.popup-content { max-height: 300px; overflow-y: auto; }
.popup-item { display: flex; align-items: center; gap: 12px; padding: 12px 16px; cursor: pointer; transition: background 0.15s; border-bottom: 1px solid var(--border-color); }
.popup-item:last-child { border-bottom: none; }
.popup-item:hover { background: var(--primary-bg); }
.popup-avatar { width: 40px; height: 40px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-weight: 600; font-size: 14px; flex-shrink: 0; background: var(--primary-blue); color: white; }
.popup-text { flex: 1; min-width: 0; }
.popup-title { font-size: 13px; font-weight: 600; margin-bottom: 2px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.popup-desc { font-size: 12px; color: var(--text-gray); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.popup-time { font-size: 11px; color: var(--text-gray); flex-shrink: 0; }
.popup-footer { padding: 12px 16px; border-top: 1px solid var(--border-color); text-align: center; }
.popup-item-icon { width: 36px; height: 36px; border-radius: 8px; display: flex; align-items: center; justify-content: center; font-size: 14px; flex-shrink: 0; }
.popup-item-icon.exam { background: rgba(37, 99, 235, 0.1); color: var(--primary-blue); }
.popup-item-icon.result { background: rgba(22, 163, 74, 0.1); color: #16a34a; }
.popup-item-icon.message { background: rgba(147, 51, 234, 0.1); color: #9333ea; }
.popup-item-icon.reminder { background: rgba(234, 88, 12, 0.1); color: #ea580c; }
"@

$cssPath = "E:\Projects\Pera\Frontend\pera-layout.css"
$currentCss = [System.IO.File]::ReadAllText($cssPath, [System.Text.Encoding]::UTF8)

if (-not ($currentCss -match "\.popup-panel")) {
    [System.IO.File]::AppendAllText($cssPath, $cssToAppend, [System.Text.Encoding]::UTF8)
    Write-Host "Appended popup styles to pera-layout.css"
} else {
    Write-Host "Popup styles already exist in pera-layout.css"
}

$standardHeader = @"
        <header class="header">
            <div class="search-bar"><i class="fa-solid fa-magnifying-glass"></i><input type="text" placeholder="Ara..." /></div>
            <div class="header-actions">
                <div class="popup-container">
                    <button class="icon-btn" onclick="togglePopup('messagesPopup')"><i class="fa-regular fa-envelope"></i></button>
                    <div id="messagesPopup" class="popup-panel">
                        <div class="popup-header"><h3>Mesajlar</h3><button class="btn-ghost btn-sm" onclick="togglePopup('messagesPopup')"><i class="fa-solid fa-xmark"></i></button></div>
                        <div class="popup-content" id="messagesPopupContent"><div class="empty-state"><p>Yükleniyor...</p></div></div>
                        <div class="popup-footer"><a href="Messages.html" class="btn-ghost btn-sm">Tüm mesajları gör</a></div>
                    </div>
                </div>
                <div class="popup-container">
                    <button class="icon-btn" onclick="togglePopup('notificationsPopup')"><i class="fa-regular fa-bell"></i><span class="badge" id="notifBadge" style="display:none;">0</span></button>
                    <div id="notificationsPopup" class="popup-panel">
                        <div class="popup-header"><h3>Bildirimler</h3><button class="btn-ghost btn-sm" onclick="togglePopup('notificationsPopup')"><i class="fa-solid fa-xmark"></i></button></div>
                        <div class="popup-content" id="notifPopupContent"><div class="empty-state"><p>Yükleniyor...</p></div></div>
                        <div class="popup-footer"><a href="Notifications.html" class="btn-ghost btn-sm">Tüm bildirimleri gör</a></div>
                    </div>
                </div>
                <div class="popup-container">
                    <div class="header-avatar" onclick="togglePopup('profilePopup')">
                        <div class="header-avatar-circle" id="headerAvatar">?</div>
                        <i class="fa-solid fa-chevron-down" style="font-size: 12px; color: var(--text-gray);"></i>
                    </div>
                    <div id="profilePopup" class="popup-panel" style="width: 260px;">
                        <div style="padding: 16px 20px; border-bottom: 1px solid var(--border-color); text-align: center;">
                            <div class="header-avatar-circle" id="popupAvatar" style="width: 56px; height: 56px; font-size: 22px; margin: 0 auto 12px;">?</div>
                            <div style="font-weight: 600; font-size: 15px;" id="popupName">-</div>
                            <div style="font-size: 13px; color: var(--text-gray); margin-top: 2px;" id="popupRole">Kullanıcı</div>
                        </div>
                        <div class="popup-content" style="padding: 8px 0;">
                            <a href="Profile.html" class="popup-item" style="display: flex; align-items: center; gap: 12px; padding: 12px 20px; color: var(--text-dark); text-decoration: none;"><i class="fa-regular fa-user" style="width: 18px; text-align: center;"></i> Profil</a>
                            <a href="Settings.html" class="popup-item" style="display: flex; align-items: center; gap: 12px; padding: 12px 20px; color: var(--text-dark); text-decoration: none;"><i class="fa-solid fa-gear" style="width: 18px; text-align: center;"></i> Ayarlar</a>
                            <div class="popup-item" onclick="localStorage.removeItem('userToken'); window.location.href='../auth/Login.html';" style="display: flex; align-items: center; gap: 12px; padding: 12px 20px; cursor: pointer; color: #dc2626;"><i class="fa-solid fa-arrow-right-from-bracket" style="width: 18px; text-align: center;"></i> Çıkış Yap</div>
                        </div>
                    </div>
                </div>
            </div>
        </header>
"@

$htmlFiles = Get-ChildItem -Path "E:\Projects\Pera\Frontend" -Recurse -Filter "*.html" | Where-Object { $_.FullName -notmatch "auth\\Login.html" -and $_.FullName -notmatch "auth\\Register.html" }

foreach ($f in $htmlFiles) {
    $content = [System.IO.File]::ReadAllText($f.FullName, [System.Text.Encoding]::UTF8)
    
    # Use Regex to replace the entire <header class="header"> ... </header> block
    $pattern = '(?s)<header class="header">.*?</header>'
    
    if ($content -match $pattern) {
        $newContent = $content -replace $pattern, $standardHeader
        
        # Also clean up duplicate popup-panel styles that might be left at the bottom of student pages
        $newContent = $newContent -replace '(?s)\.popup-panel \{.*?\.popup-panel\.active \{.*?\}', ''
        
        [System.IO.File]::WriteAllText($f.FullName, $newContent, [System.Text.Encoding]::UTF8)
        Write-Host "Standardized header in: $($f.Name)"
    }
}
