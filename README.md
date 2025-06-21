# 🛡️ Avventura Testuale di Zelda
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)

Un semplice gioco di avventura testuale ispirato a The Legend of Zelda, creato in C#. Questo progetto è stato sviluppato come esercizio per approfondire i concetti della programmazione orientata agli oggetti.

## ✨ Features
* **Mondo di gioco persistente:** la mappa del castello viene caricata dinamicamente da un file di testo (`Rooms.txt`).
* **Esplorazione interattiva:** usa comandi semplici per interagire con l'ambiente.
* **Sistema di combattimento:** affronta i mostri che bloccano il cammino usando le armi corrette.
* **Architettura orientata agli oggetti:** codice pulito e modulare grazie a classi dedicate per `Player`, `Room`, `Item` e `Monster`.

## 🚀 Come eseguire il progetto
Per compilare ed eseguire il gioco sul tuo computer, avrai bisogno di:
1.  [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (o superiore).
2.  Clonare questo repository.
3.  Navigare nella cartella del progetto ed eseguire il comando:
    ```bash
    dotnet run
    ```

## 🎮 Comandi di gioco
Controlla l'eroe con i seguenti comandi:

| Comando             | Esempio             | Descrizione                               | Stato Attuale |
| ------------------- | ------------------- | ----------------------------------------- |:-------------:|
| `LOOK`              | `LOOK`              | Descrive la stanza attuale e le uscite.   | ✅ **Attivo** |
| `MOVE <direzione>`  | `MOVE NORTH`        | Ti sposta in una nuova stanza.            | ✅ **Attivo** |
| `PICK <oggetto>`    | `PICK DAGGER`       | Raccoglie un oggetto dalla stanza.        | ✅ **Attivo** |
| `DROP <oggetto>`    | `DROP CROSS`        | Lascia un oggetto nella stanza.           | ✅ **Attivo** |
| `ATTACK`            | `ATTACK`            | Attacca il mostro presente nella stanza.  | ✅ **Attivo** |
| `INVENTORY` / `I`   | `INVENTORY`         | Mostra il contenuto della borsa.          | ✅ **Attivo** |
| `EXIT`              | `EXIT`              | Termina la partita.                       | ✅ **Attivo** |


## 📋 Prossimi Passi e Finalizzazione
Per completare tutti i requisiti del progetto, mancano i seguenti passaggi:

- [X] Aggiungere una proprietà `HasRescuedPrincess` alla classe `Player` per tenere traccia del salvataggio.
- [X] Definire la stanza finale in cui si trova la principessa (es. Stanza 8 o 9).
- [X] Modificare la logica di movimento per impostare `HasRescuedPrincess` a `true` quando il giocatore entra nella stanza finale.
- [X] Potenziare il comando `EXIT` per controllare lo stato di `HasRescuedPrincess` e mostrare il messaggio di vittoria (`EndWin.txt`) o di sconfitta (`EndLose.txt`) corretto.

---
Sviluppato da **Marco Morello**