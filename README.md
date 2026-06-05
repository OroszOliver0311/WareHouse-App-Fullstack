## Követelmény specifikáció
Az alkalmazás célja több telephelyes raktárkészlet nyilvántartása. A rendszer lehetővé teszi a termékek raktárankénti követését és a készletmozgások transzparens naplózását.

### Funkcionális követelmények
Beléptetés: Egyszerűsített autentikáció a rendszer eléréséhez.(Angular keretrendszer használatával)
Termék Dashboard: A belépés után megjelenik az összes termék listája (Név, SKU, összesített készlet).
Készletkezelés (Részletes nézet): Egy termékre kattintva a felhasználó látja a termék adatait, valamint egy lebontást, hogy melyik raktárhelyszínen mekkora készlet érhető el.
    A helyszínek mellett található beviteli mezővel és +/- gombokkal módosítható a készlet.
    A módosítás automatikusan új készletmozgást generál a háttérben.
Előzmények: A részletes nézetből elérhető egy előzmény ablak, amely időrendben listázza az adott termékhez kapcsolódó összes tranzakciót (típus, mennyiség, dátum, raktár megnevezése).
Adminisztráció: Új termékek és új raktárhelyszínek felvétele.

## Adatbázis entitások

Raktár: ID, Név, Cím
Termék: ID, Név, SKU, Egységár
RaktárTermék (összekapcsoló tábla): ID, TermékId, RaktárId, Készletmennyiség
Termékmozgás: ID, RaktárTermékId, Típus(+/-), Mennyiség, Dátum 

## Alkalmazott alaptechnológiák

Adatbázis: MS SQL Server
Adatelérés: Entity Framework Core v10.x
Backend szolgáltatás: ASP.NET Core v10.x
Kliens: Angular

## NHF Megszerezni kívánt pontok

adatbázis index konfigurációja az EF modellben  
verziókezelt API. Szemléltetés két különböző verziós API egyidejű kiszolgálásával. A kívánt verziót HTTP fejléc vagy például URL szegmens alapján választhatja meg a kliens.   
az EF Core működőképességét, az adatbázis elérhetőségét jelző health check végpont publikálása a Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore NuGet csomag használatával.  
külső komponens használata DTO-k inicializálásáraObject mapper, pl. AutoMapper  
minimum 10 függvényhez/végponthoz  
a unit tesztekben a mock objektumok injektálása  
EF Core memória-adatbázis vagy sqlite (vagy in-memory sqlite) használata teszteléshez  
adatbetöltés (seeding) migráció segítségével  
saját Code-First konvenció készítése   
Dátumok helyes kezelése olyan esetben is, ha a kliens és a szerver(ek) eltérő időzónában vannak. (tipikusan DateTimeOffset használata). Implementációt nem kell bemutatáskör demonstrálni, de a kód alapján érvelve bizonyítani kell a helyes működést.  
Központosított hibakezelés, a kliens számára értelmezhető ProblemDetails objektumok küldése hibás kérések esetén. Tehát nem a Controllerekben van try-catch, hanem a hibák központilag vannak kezelve  
Adatbázis entitás elsődleges kulcs elrejtése a kliens elől véletlenszerűen generált, nem növekvő sorrendben kiosztott kulcsokkal. A kliens nem ismeri az adatbázis entitás kulcs értékét, helyette egy generált kulcsot lát csak. Az adatbázis nem tárolja a generált kulcsot. Megvalósítható kétirányú szám <-> generált azonosító függvények  
az API-nak egyidejűleg több támogatott verziója van, mindegyik dokumentált és mindegyik támogatott verzió dokumentációja elérhető   
A kliensen az OpenApi leíró alapján generált klienskönyvtár használata  
Command-line generátor eszköz használata és konfigurálása
minden végpont kliens szempontjából releváns működése dokumentált, minden lehetséges válaszkóddal együtt. 



