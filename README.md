# florenBooks - Sistem de Management Bibliotecă

## Credentiale Login
- **Username:** `admin`
- **Parolă:** `admin123`

---

## Structura Proiectului

| Fișier | Descriere |
|--------|-----------|
| `FormLogin.cs` | Ecran autentificare |
| `Home.cs` | Meniu principal (MenuStrip cu toate modulele) |
| `BookRegistration.cs` | Înregistrare cărți noi |
| `BookInformation.cs` | Vizualizare / Editare / Ștergere cărți |
| `MemberRegistration.cs` | Înregistrare membri noi |
| `MemberInformation.cs` | Vizualizare / Editare / Ștergere membri |
| `BookIssue.cs` | Împrumut carte |
| `BookReturn.cs` | Returnare carte + calcul penalizare |
| `ViewIssues.cs` | Vizualizare toate împrumuturile |
| `DatabaseHelper.cs` | Helper centralizat conexiune DB |
| `CreateDatabase.sql` | Script SQL creare baza de date |

---

## Pași Setup

### 1. Creare Baza de Date

1. Deschide **SQL Server Management Studio (SSMS)**
2. Conectează-te la `(LocalDB)\MSSQLLocalDB`
3. Modifică căile din `CreateDatabase.sql` (linia 18-27) cu locația ta reală
4. Rulează scriptul `CreateDatabase.sql`

### 2. Configurare Conexiune

Deschide `DatabaseHelper.cs` și modifică `ConnectionString`:

```csharp
// Varianta cu cale absoluta:
@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\CALEA_TA\florenBooks\library.mdf;Integrated Security=True"

// Varianta cu |DataDirectory| (dupa build, pune .mdf langa .exe):
@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\library.mdf;Integrated Security=True"
```

### 3. Restaurare NuGet Packages

```bash
dotnet restore
```

### 4. Build și Rulare

```bash
dotnet run
```

sau apasă **F5** în Visual Studio.

---

## Adăugare Controale din Toolbox

Fiecare formular are un comment detaliat în `.cs` cu lista de controale necesare.
Fiecare `.Designer.cs` declară câmpurile `internal` care trebuie conectate.

### Pași pentru fiecare formular:
1. Deschide formularul în **Designer** (dublu click pe `.cs` în Solution Explorer)
2. Din **Toolbox** trage controalele necesare (TextBox, Button, Label, etc.)
3. În **Properties** → schimbă `(Name)` la numele din comentariu (ex: `textBoxTitle`)
4. Conectează evenimentele: în **Properties → Events (⚡)** → dublu click pe eveniment

### Evenimente de conectat:

| Formular | Control | Eveniment | Handler |
|----------|---------|-----------|---------|
| FormLogin | buttonLogin | Click | `buttonLogin_Click` |
| FormLogin | buttonCancel | Click | `buttonCancel_Click` |
| FormLogin | textBoxPassword | KeyPress | `textBoxPassword_KeyPress` |
| BookRegistration | Form | Load | `BookRegistration_Load` |
| BookRegistration | buttonSave | Click | `buttonSave_Click` |
| BookRegistration | buttonClear | Click | `buttonClear_Click` |
| BookInformation | Form | Load | `BookInformation_Load` |
| BookInformation | buttonSearch | Click | `buttonSearch_Click` |
| BookInformation | buttonUpdate | Click | `buttonUpdate_Click` |
| BookInformation | buttonDelete | Click | `buttonDelete_Click` |
| MemberRegistration | Form | Load | `MemberRegistration_Load` |
| MemberRegistration | buttonSave | Click | `buttonSave_Click` |
| MemberInformation | Form | Load | `MemberInformation_Load` |
| MemberInformation | buttonSearch | Click | `buttonSearch_Click` |
| MemberInformation | buttonUpdate | Click | `buttonUpdate_Click` |
| MemberInformation | buttonDelete | Click | `buttonDelete_Click` |
| BookIssue | Form | Load | `BookIssue_Load` |
| BookIssue | textBoxMemberId | Leave | `textBoxMemberId_Leave` |
| BookIssue | textBoxBookId | Leave | `textBoxBookId_Leave` |
| BookIssue | buttonIssue | Click | `buttonIssue_Click` |
| BookReturn | Form | Load | `BookReturn_Load` |
| BookReturn | buttonSearch | Click | `buttonSearch_Click` |
| BookReturn | buttonCalculate | Click | `buttonCalculate_Click` |
| BookReturn | buttonReturn | Click | `buttonReturn_Click` |
| ViewIssues | Form | Load | `ViewIssues_Load` |
| ViewIssues | buttonSearch | Click | `buttonSearch_Click` |
| ViewIssues | buttonShowAll | Click | `buttonShowAll_Click` |
| ViewIssues | buttonShowActive | Click | `buttonShowActive_Click` |
| ViewIssues | buttonShowReturned | Click | `buttonShowReturned_Click` |
| Home | (MenuStrip items) | Click | handler-ele corespunzatoare |

---

## Schema Baza de Date

```
book           member          book_issue
─────────────  ──────────────  ─────────────────────
id (PK)        id (PK)         id (PK)
title          name            member_id
author         gender          member_name
publisher      phone           book_id
year           email           book_title
isbn           address         book_author
category       date_joined     issue_date
quantity       member_type     due_date
price          max_books       return_date
shelf                          fine_per_day
                               total_fine
                               status
```
