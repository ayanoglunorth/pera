const fs = require('fs');
const path = require('path');

const directories = [
    'E:\\Projects\\Pera\\Frontend\\teacher',
    'E:\\Projects\\Pera\\Frontend\\student'
];

directories.forEach(dir => {
    fs.readdirSync(dir).forEach(file => {
        if (!file.endsWith('.html')) return;
        const filepath = path.join(dir, file);
        let content = fs.readFileSync(filepath, 'utf8');

        // Fix ReferenceError in popupRole logic
        content = content.replace(
            /currentUserRole\s*===\s*"Teacher"/g, 
            '(user.role || user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]) === "Teacher"'
        );

        // Fix missing </script> in Grades.html
        if (file === 'Grades.html' && dir.includes('teacher')) {
            if (!content.includes('</script>\n</body>') && !content.includes('</script>\r\n</body>')) {
                content = content.replace('</body>', '</script>\n</body>');
            }
        }

        // Fix header text in student/Exams.html
        if (file === 'Exams.html' && dir.includes('student')) {
            content = content.replace('<h1 class="page-title">Öğrenciler</h1>', '<h1 class="page-title">Sınavlarım</h1>');
            content = content.replace('<p class="page-subtitle">Platforma kayıtlı tüm öğrenciler.</p>', '<p class="page-subtitle">Sınav sonuçların ve yaklaşan sınavlar.</p>');
        }

        fs.writeFileSync(filepath, content, 'utf8');
        console.log("Patched:", filepath);
    });
});
