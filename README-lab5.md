# ShopApp - Sistem de Gestionare Email

#lab5

## Descriere

ShopApp include o aplicație pentru gestionarea email-urilor folosind protocoalele **POP3**, **IMAP** și **SMTP**. Aceasta permite utilizatorilor să vizualizeze, să descarce și să trimită email-uri cu sau fără atașamente, precum și să gestioneze subiectele și răspunsurile.

---

## Funcționalități

1. **Vizualizarea listei de email-uri din inbox folosind POP3**  
   - Protocolul POP3 este utilizat pentru a accesa mesajele din cutia poștală.  
   - **Nivel de prioritate:** **2**

2. **Vizualizarea listei de email-uri din inbox folosind IMAP**  
   - Protocolul IMAP este utilizat pentru navigarea și gestionarea email-urilor direct pe server.  
   - **Nivel de prioritate:** **2**

3. **Descărcarea email-urilor și a atașamentelor**  
   - Utilizatorii pot descărca atașamentele din email-uri direct din interfață.  
   - **Nivel de prioritate:** **2**

4. **Trimiterea unui email cu text simplu**  
   - Funcționalitate de bază pentru trimiterea unui mesaj fără atașamente.  
   - **Nivel de prioritate:** **1**

5. **Trimiterea unui email cu atașamente**  
   - Utilizatorii pot adăuga fișiere atașate la email-urile trimise.  
   - **Nivel de prioritate:** **2**

6. **Trimiterea unui email cu opțiuni avansate (subiect și reply-to)**  
   - La trimiterea unui email, utilizatorii pot specifica subiectul și detalii de tip **reply-to**.  
   - **Nivel de prioritate:** **1**

---

## Cum funcționează

### 1. Autentificare și Logare
- **Autentificare**: Utilizatorii trebuie să se logheze cu un cont valid prin pagina de logare.
- **Autorizare**: Accesul la inbox este permis doar utilizatorilor autentificați.
- **User Manager**: Utilizăm `UserManager` pentru a obține detalii despre utilizatorul curent.

```csharp
var state = await AuthState.GetAuthenticationStateAsync();
var user = state.User;
if (!user.Identity?.IsAuthenticated ?? true)
{
    Nav.NavigateTo("/Account/Login");
    return;
}
```

---

### 2. Cum funcționează vizualizarea email-urilor folosind IMAP
- **Protocolul IMAP** permite accesarea cutiei poștale direct de pe server, fără a descărca mesajele.
- Interfața afișează email-urile utilizând un tabel care include:
  - Expeditorul
  - Subiectul
  - Conținutul
  - Opțiuni de descărcare a atașamentelor

```csharp
var result = await _imapService.GetInboxAsync(email, password);
if (result == null || result.Count == 0)
    return NotFound("No messages found or authentication failed.");
```

---

### 3. Cum funcționează vizualizarea email-urilor folosind POP3
- **Protocolul POP3** este utilizat pentru descărcarea mesajelor din inbox.
- Mesajele sunt descărcate utilizând un serviciu POP3 specializat.

```csharp
var emails = await _pop3Service.GetInboxPop3Async(userEmail, password);
return Ok(emails);
```

---

### 4. Cum funcționează trimiterea email-urilor folosind SMTP
- **Protocolul SMTP** permite trimiterea mesajelor prin serviciul de email.
- Mesajele includ:
  - Text simplu sau text + atașamente
  - Subiectul mesajului
  - Detalii de tip **reply-to**

```csharp
var dto = new EmailMessageDTO
{
    From = _config["SmtpSettings:Username"],
    To = email,
    Subject = "Confirm your email",
    Body = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.",
    IsHtml = true
};
await _customEmailService.SendEmailAsync(dto);
```

---

## Testing - Cum testăm în Swagger

### Testare IMAP
1. Navigați la endpoint-ul:
   ```
   GET /api/email/inbox-imap
   ```
2. Introduceți următorii parametri:
   - **email**: Adresa de email a utilizatorului.
   - **password**: Parola asociată cutiei poștale.
3. Executați cererea pentru a obține lista de email-uri din inbox.

---

### Testare POP3
1. Navigați la endpoint-ul:
   ```
   GET /api/email/inbox-pop3
   ```
2. Introduceți următorii parametri:
   - **userEmail**: Adresa de email a utilizatorului.
   - **password**: Parola asociată cutiei poștale.
3. Executați cererea pentru a vizualiza mesajele descărcate.

---

### Testare SMTP
1. Navigați la endpoint-ul:
   ```
   POST /api/email/send
   ```
2. Introduceți un `JSON` cu următoarele câmpuri:
   ```json
   {
       "from": "your-email@example.com",
       "to": "recipient@example.com",
       "subject": "Test Email",
       "body": "This is a test email.",
       "isHtml": false
   }
   ```
3. Executați cererea pentru a trimite un email.

---

## Tehnologii utilizate

- **Frontend**: Blazor, MudBlazor
- **Backend**: ASP.NET Core
- **Protocoale**: POP3, IMAP, SMTP
- **Limbaje**: C#, HTML, CSS
**.