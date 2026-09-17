# ResearchFlow

ASP.NET Core MVC على .NET 10 لإدارة الدراسات والمراحل والإجراءات البحثية.

## التشغيل
1. ثبّت .NET 10 SDK وSQL Server LocalDB.
2. نفّذ `dotnet --version` وتأكد أن الإصدار يبدأ بـ 10.
3. نفّذ `dotnet restore`.
4. نفّذ `dotnet build`.
5. نفّذ `dotnet run`.
6. افتح رابط HTTPS الظاهر.

## حسابات العرض
كلمة المرور الموحدة: `Lab@2026`
- manager@researchflow.local
- researcher1@researchflow.local
- researcher2@researchflow.local
- assistant@researchflow.local

## ملاحظة SQL Express
استبدل `(localdb)\MSSQLLocalDB` في appsettings.json بـ `.\SQLEXPRESS` إذا لزم.
