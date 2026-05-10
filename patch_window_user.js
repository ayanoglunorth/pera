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

        // Inject window.user and window.currentUserRole to fix ReferenceError
        const injection = `const user = parseJwt(token);
            window.user = user;
            window.currentUserRole = user.role || user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];`;
            
        content = content.replace(/const user = parseJwt\(token\);/g, injection);

        fs.writeFileSync(filepath, content, 'utf8');
        console.log("Patched window.user in:", filepath);
    });
});
