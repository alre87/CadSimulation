# Console Application CadSimulation.ConsoleApp

L'applicazione principale è CadSimulation.ConsoleApp.  
Nel file launchSettings.json sono presenti tutte le configurazioni per lesecuzione da Visual Studio.  
In caso di doppio repository (File e Http) viene preferito il salvataggio su file.  
l'applicazione richiede almeno un repository di salvataggio (path o uri) mentre il parametro Json è opzionale

## Elenco Parametri accettati

- --path [FullPath]

  Viene richiesto subito dopo il percorso completo di filename
- --json

  Usato per forzare utilizzo del formato Json
- uri [ServiceUrl]

  dev'essere indicato subito dopo l'uri del servizio Http

## Utilizzo sel repository File FileSystem

Al momento l'applicazione è impostata per l'utilizzo della cartella C:\temp\

## Utilizzo sel repository Http

Nel file shapes_server.js (Fornito) è presente il servizio in NodeJs che simula il servizio Http che dev'essere avvito separatemente.

## Esempi di lancio dell'applicazione

- Leggere e scrivere i dati su FileSystem in formato Custom
  - Esempio:

  ``` cmd
  CadSimulation.ConsoleApp.exe --path c:\temp\shapes.txt
  ```

- Leggere e scrivere i dati FileSystem in formato Json
  - Esempio:
  
  ``` cmd
  CadSimulation.ConsoleApp.exe --path c:\temp\shapes.txt --json
  ```

- Leggere e scrivere i dati su Http in formato Custom
  - Esempio:
  
  ``` cmd
  CadSimulation.ConsoleApp.exe --uri http://127.0.0.1:8787/shapes
    ```

- Leggere e scrivere i dati Http in formato Json
  - Esempio:
  
  ``` cmd
  CadSimulation.ConsoleApp.exe --uri http://127.0.0.1:8787/shapes --json
  ```
