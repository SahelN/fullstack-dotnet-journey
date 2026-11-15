# Databaser – Övning 3: Lastbalansering

## 1. Vad innebär lastbalansering?
Lastbalansering betyder att man delar upp arbetet på flera delar. Till exempel i en affär, om det finns flera kassor kan man betala snabbt och det blir ingen lång kö. Det blir inte heller för mycket arbete för en kassa. Med flera kassor sparar man tid och arbetet för varje personal blir lättare. Om det händer något med en kassa kan man byta plats utan problem.

## 2. Exempel där lastbalansering är bra
Om man har en populär webbsida som många använder live och samtidigt över hela världen, då är det bra att ha flera servrar.

## 3. Tre fördelar för en hemsida
1. Webbsidan blir inte långsam om många använder den samtidigt.
2. Om det händer något med en server kan de andra fortsätta jobba, utan att användarna märker problemet.
3. Om man får dubbelt så många besökare lägger man bara till fler servrar med samma struktur.

## 4. Vad är SQL Server Replication?
Det betyder att man kan kopiera och distribuera en databas till andra databaser. När originalet ändras skickas ändringarna till de andra databaserna.

## 5. Vad är Mirroring i SQL Server?
Nästan som replication, men man kan inte läsa och skriva från backup-versionen. Data går bara från originaldatabasen till backup-versionen. Det är bra om det händer något med originalet, för då tar kopian enkelt dess plats.

## 6. Vad är AlwaysOn Availability Groups i SQL Server?
AlwaysOn är nästan som mirroring men en bättre version. Man kan ha flera backup, inte bara en. Man kan läsa från backup-versionen, men inte skriva.
