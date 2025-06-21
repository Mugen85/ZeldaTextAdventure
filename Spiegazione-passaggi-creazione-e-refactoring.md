# Documentazione del Progetto: Avventura Testuale di Zelda

## 1. Introduzione e scopo

Questo documento ripercorre le fasi di sviluppo di un'avventura testuale in C#, creata da zero con l'obiettivo di applicare i fondamenti della programmazione orientata agli oggetti (OOP) e delle buone pratiche di architettura software. Il progetto simula un'avventura in un castello dove l'obiettivo è sconfiggere mostri, raccogliere oggetti e salvare la principessa.

## 2. Riepilogo delle fasi di sviluppo

Il progetto è stato sviluppato attraverso 7 fasi logiche principali:

1.  impostazione iniziale e struttura di base
2.  implementazione delle meccaniche di base
3.  interazione con gli oggetti
4.  introduzione dei mostri e del combattimento
5.  logica di gioco finale
6.  refactoring architettonico
7.  migrazione a JSON e pulizia finale

## 3. Dettaglio delle fasi

### Fase 1: impostazione iniziale
- **attività:**
    - creato un nuovo progetto Console App in C#.
    - definite le classi-modello iniziali: `Player`, `Room`, `Item`, `Monster` con le loro proprietà essenziali.
    - impostata la lettura dei dati da file di testo (`.txt`) per la storia e le stanze.
- **concetti appresi:**
    - struttura di un progetto C#, definizione di classi e proprietà, I/O di base (`File.ReadAllText`).

### Fase 2: meccaniche di base
- **attività:**
    - implementato il `GameLoop` (`while (isPlaying)`), il cuore pulsante del gioco.
    - sviluppato il comando `LOOK` per descrivere l'ambiente.
    - sviluppato il comando `MOVE` per permettere lo spostamento tra le stanze, gestendo le uscite valide e i muri.
- **concetti appresi:**
    - gestione di un ciclo di gioco, input dell'utente (`Console.ReadLine`), strutture di controllo (`switch`), uso di `Dictionary` per le uscite.

### Fase 3: interazione con gli oggetti
- **attività:**
    - aggiunta la proprietà `Bag` (un `List<Item>`) alla classe `Player`.
    - implementato il comando `PICK` per spostare oggetti dalla stanza all'inventario.
    - implementato il comando `DROP` per spostare oggetti dall'inventario alla stanza.
    - implementato il comando `INVENTORY` (e la scorciatoia `I`) per visualizzare il contenuto della borsa.
- **concetti appresi:**
    - gestione di collezioni (`List`), uso di metodi LINQ come `FirstOrDefault` e `Any` per cercare oggetti.

### Fase 4: mostri e combattimento
- **attività:**
    - ampliata la classe `Monster` con proprietà come `Weakness` e logica di sblocco uscite.
    - creato un sistema di dati più robusto con un file `Monsters.txt` dedicato.
    - aggiornato `LoadGameData` per caricare e collegare i mostri alle stanze.
    - implementato il comando `ATTACK`, che controlla l'inventario per l'arma corretta e determina la vittoria (sbloccando un'uscita) o la sconfitta (game over).
- **concetti appresi:**
    - design di meccaniche di gioco, gestione di stati (mostro `IsAlive`), modifica dinamica dello stato del mondo (aggiunta di uscite).

### Fase 5: logica di gioco finale
- **attività:**
    - aggiunta la proprietà `HasRescuedPrincess` alla classe `Player`.
    - modificato il comando `MOVE` per impostare la proprietà a `true` all'ingresso nella stanza finale.
    - potenziato il comando `EXIT` per leggere file di finale diversi (`EndWin.txt`, `EndLose.txt`) in base allo stato di `HasRescuedPrincess` e alla posizione del giocatore.
- **concetti appresi:**
    - uso di "flag" booleani per tracciare lo stato del gioco, implementazione di logica condizionale complessa.

### Fase 6: refactoring architettonico
- **attività:**
    - organizzati i file del progetto in cartelle (`Models`) per una migliore struttura.
    - estratta la logica di interpretazione dell'input in una classe `CommandParser` dedicata, introducendo `enum` e classi `Command` per un codice più sicuro e pulito.
    - isolata la logica di gioco in una classe `GameEngine`, mentre `Program.cs` è diventato il coordinatore del gioco.
- **concetti appresi:**
    - Principio di Singola Responsabilità (SRP), refactoring, disaccoppiamento del codice, importanza dei `namespace`, differenza tra `static` e istanza.

### Fase 7: migrazione a JSON e pulizia finale
- **attività:**
    - sostituiti tutti i file di dati `.txt` con un unico file `GameData.json` centralizzato.
    - riscritto il metodo `LoadGameData` per usare la deserializzazione JSON (`System.Text.Json`).
    - aggiornate le classi modello con attributi `[JsonPropertyName]` per un mapping corretto.
    - risolti tutti gli errori e gli avvisi del compilatore, inclusi quelli relativi alla gestione dei valori `null`.
- **concetti appresi:**
    - serializzazione/deserializzazione JSON, attributi C#, gestione avanzata della "null-safety", importanza della pulizia del codice.