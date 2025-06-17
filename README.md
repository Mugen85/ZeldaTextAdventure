# 🛡️ Avventura Testuale di Zelda
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)

Un semplice gioco di avventura testuale ispirato a The Legend of Zelda, creato in C#. Questo progetto è stato sviluppato come esercizio per approfondire i concetti della programmazione orientata agli oggetti.

## ✨ Features
* **Mondo di gioco persistente:** la mappa del castello viene caricata dinamicamente da un file di testo (`Rooms.txt`).
* **Esplorazione interattiva:** usa comandi semplici per interagire con l'ambiente.
* **Architettura orientata agli oggetti:** codice pulito e modulare grazie a classi dedicate per `Player`, `Room`, `Item` e `Monster`.

## 🚀 Come eseguire il progetto
Per compilare ed eseguire il gioco sul tuo computer, avrai bisogno di:
1.  [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (o superiore).
2.  Clonare questo repository.
3.  Navigare nella cartella del progetto ed eseguire il comando:
    ```bash
    dotnet run
    ```

## 🎮 Comandi di gioco
Controlla l'eroe con i seguenti comandi:

| Comando             | Esempio             | Descrizione                               | Stato Attuale |
| ------------------- | ------------------- | ----------------------------------------- |:-------------:|
| `LOOK`              | `LOOK`              | Descrive la stanza attuale e le uscite.   | ✅ **Attivo** |
| `MOVE <direzione>`  | `MOVE NORTH`        | Ti sposta in una nuova stanza.            | ✅ **Attivo** |
| `PICK <oggetto>`    | `PICK SWORD`        | Raccoglie un oggetto dalla stanza.        | ⏳ Prossimo   |
| `DROP <oggetto>`    | `DROP SHIELD`       | Lascia un oggetto nella stanza.           | ❌ Inattivo   |
| `ATTACK`            | `ATTACK`            | Attacca il mostro presente nella stanza.  | ❌ Inattivo   |
| `EXIT`              | `EXIT`              | Termina la partita.                       | ✅ **Attivo** |


## 📋 Stato del progetto
Ecco un riepilogo più dettagliato dello stato di avanzamento:

- [X] Creazione delle classi di base (`Player`, `Room`, `Item`, `Monster`).
- [X] Lettura della storia iniziale da file di testo.
- [X] Caricamento dinamico del mondo di gioco da `Rooms.txt`.
- [X] Implementazione comando `LOOK`.
- [X] Implementazione comando `EXIT`.
- [X] Implementazione comando `MOVE`.
- [ ] Implementazione comandi `PICK` e `DROP` per la gestione dell'inventario.
- [ ] Implementazione comando `ATTACK` e logica di combattimento.
- [ ] Logica di vittoria e sconfitta.

---
Sviluppato da **Marco Morello**
