# ShopApp

#lab4 - Tema: Aplicație de tip client HTTP

## Descriere

ShopApp este o aplicație pentru gestionarea categoriilor și produselor într-un magazin virtual. Această aplicație simplifică procesul de administrare a categoriilor și produselor, oferind funcționalități esențiale pentru utilizatori.

---

## Funcționalități

### Categorii

1. **Listarea categoriilor**  
   - Aplicația poate afișa o listă cu toate categoriile disponibile.

2. **Detalii despre o categorie**  
   - Aplicația permite vizualizarea detaliilor unei categorii specifice.

3. **Crearea unei categorii noi**  
   - Aplicația oferă posibilitatea de a adăuga o categorie nouă.  
   - Nivel de prioritate: **2**

4. **Ștergerea unei categorii**  
   - Aplicația permite ștergerea unei categorii existente.  
   - Nivel de prioritate: **1**

5. **Modificarea titlului unei categorii**  
   - Aplicația permite actualizarea titlului unei categorii.  
   - Nivel de prioritate: **2**

---

### Produse

1. **Adăugarea de produse noi într-o categorie**  
   - Utilizatorii pot adăuga produse noi asociate unei categorii specifice.  
   - **Nivel de prioritate:** **2**

2. **Vizualizarea listei de produse dintr-o categorie**  
   - Aplicația permite afișarea tuturor produselor asociate unei categorii.  
   - **Nivel de prioritate:** **1**

---

## Implementare

### Interfață utilizator
- Utilizarea framework-ului **MudBlazor** pentru crearea unei interfețe moderne și responsive.
- **Dialoguri interactive** pentru adăugarea, editarea și vizualizarea datelor.

### Backend
- Backend-ul este construit folosind **ASP.NET Core** și expune un set de API-uri REST pentru gestionarea categoriilor și produselor.
- Servicii disponibile pentru operații CRUD, cum ar fi crearea, citirea, actualizarea și ștergerea datelor.

### Funcționalități tehnice
- **Mapări DTO** pentru a facilita transferul de date între client și server.
- Conexiune cu baza de date pentru gestionarea persistentă a datelor despre categorii și produse.

---

## Ghid de utilizare

### Pornirea aplicației

1. Clonați repository-ul:
   ```bash
   git clone https://github.com/horozova52/ShopApp.git
   ```
2. Deschideți proiectul în **Visual Studio** sau editorul preferat.
3. Configurați baza de date în fișierul `appsettings.json`.
4. Rulați aplicația:
   ```bash
   dotnet run
   ```

### Utilizarea aplicației

- **Categorii**: Navigați la secțiunea categorii pentru a adăuga, edita sau șterge categorii.
- **Produse**: Utilizați funcționalitatea de asociere a produselor cu categoriile pentru o administrare eficientă.

---

## API-uri , care pot fi testate in Swagger

### Categorii
- `GET /api/category` - Obține toate categoriile.
- `GET /api/category/{id}` - Detalii despre o categorie.
- `POST /api/category` - Creează o categorie nouă.
- `PUT /api/category/{id}` - Actualizează o categorie.
- `DELETE /api/category/{id}` - Șterge o categorie.
- `GET /api/category/{id}/books` - Obține lista de produse dintr-o categorie.

### Produse
- `GET /api/book` - Obține toate produsele.
- `GET /api/book/{id}` - Detalii despre un produs.
- `POST /api/book` - Creează un produs.
- `PUT /api/book/{id}` - Actualizează un produs.
- `DELETE /api/book/{id}` - Șterge un produs.

---