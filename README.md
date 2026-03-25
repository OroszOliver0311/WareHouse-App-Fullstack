# Szoftverfejlesztés .NET platformra - Házi feladat

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