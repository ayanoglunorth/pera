const token = await fetch('http://localhost:5268/api/Auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email: 'ayse.yilmaz@pera.com', password: 'Pera2024!' })
}).then(res => res.json()).then(data => data.token);

console.log("Token:", token.substring(0, 20) + "...");

const res = await fetch('http://localhost:5268/api/Dashboard', {
    headers: { 'Authorization': `Bearer ${token}` }
});
console.log("Dashboard status:", res.status);
if (res.status === 200) {
    console.log("Dashboard data:", await res.json());
} else {
    console.log("Dashboard error text:", await res.text());
}
