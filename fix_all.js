const fs = require('fs');
const path = require('path');

const standardHeader = `
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
        </header>`;

const directories = [
    'E:\\Projects\\Pera\\Frontend\\teacher',
    'E:\\Projects\\Pera\\Frontend\\student'
];

// Restore correct UTF-8 strings
const fixes = [
    { bad: "Ã‡Ä±kÄ±ÅŸ Yap", good: "Çıkış Yap" },
    { bad: "SÄ±navlarÄ±m", good: "Sınavlarım" },
    { bad: "NotlarÄ±m", good: "Notlarım" },
    { bad: "MesajlarÄ±", good: "Mesajları" },
    { bad: "Mesajlari", good: "Mesajları" } // Ensure this is also correct
];

const jsPopupProfileFix = `
            // Header avatar & popup user data
            const headerAvatar = document.getElementById("headerAvatar");
            if (headerAvatar) headerAvatar.textContent = firstLetter;
            
            const popupAvatar = document.getElementById("popupAvatar");
            if (popupAvatar) popupAvatar.textContent = firstLetter;
            
            const popupName = document.getElementById("popupName");
            if (popupName) popupName.textContent = userName;
            
            const popupRole = document.getElementById("popupRole");
            if (popupRole) popupRole.textContent = currentUserRole === "Teacher" ? "Öğretmen" : "Öğrenci";
`;

directories.forEach(dir => {
    fs.readdirSync(dir).forEach(file => {
        if (!file.endsWith('.html')) return;
        const filepath = path.join(dir, file);
        let content = fs.readFileSync(filepath, 'utf8');

        // Fix encoding errors
        fixes.forEach(f => {
            content = content.split(f.bad).join(f.good);
        });

        // Replace header
        const headerRegex = /<header class="header">[\s\S]*?<\/header>/;
        content = content.replace(headerRegex, standardHeader);

        // Inject JS popup profile logic if it's missing (check if popupName is populated)
        if (!content.includes('popupName.textContent = userName')) {
            // Find where headerAvatar is populated and replace it
            const avatarRegex = /const headerAvatar = document\.getElementById\("headerAvatar"\);\s*if \(headerAvatar\) headerAvatar\.textContent = firstLetter;/;
            if (avatarRegex.test(content)) {
                content = content.replace(avatarRegex, jsPopupProfileFix);
            }
        }

        fs.writeFileSync(filepath, content, 'utf8');
        console.log("Processed:", file);
    });
});
