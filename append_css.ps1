
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

[System.IO.File]::AppendAllText("E:\Projects\Pera\Frontend\pera-layout.css", $cssToAppend, [System.Text.Encoding]::UTF8)
Write-Host "Appended successfully!"
