# PWT - Frontend Test

## Opgavebeskrivelse:
Udvikl en Blazor WebAssembly-app eller Blazor Server og byg et simpelt interface, der henter og viser kun den vigtige data om varer og deres beholdning fra en backend-service.

## Opgavekrav:
### 1. Frontend:
- Lav en overskuelig liste eller tabel, hvor varer og deres beholdning vises.
- Brug CSS og HTML til at style komponenterne.
- Inkluder søge- og/eller filterfunktionalitet på listen.

### 2. Backend:
- Opret en minimal API (fx i ASP.NET Core) eller en serviceklasse, der henter data via den angivne connectionstring.
- Simuler evt. CRUD-operationer, men det er tilstrækkeligt, at der er read-operationer.

### 3. Data:
- Brug en connectionstring (hardcoded i appsettings.json) til database. Dataene skal komme fra tabellerne for varer og beholdning – ikke egen datasets.

### 4. Unittest:
- Implementer en eller to enkle unittests (fx med xUnit eller bUnit), der tester den frontend-service der præsenterer data.

# Connection String
Connectionstring:
server=string.pwtgroup.dk,1491;database=TestDB;MultipleActiveResultSets=True;user=TestU
ser;password=!"#123!"#abcABC
