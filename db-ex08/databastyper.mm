<map version="1.0.1">
<node TEXT="Typer av databaser">
<node TEXT="RDBMS" POSITION="right" COLOR="#0033cc" FOLDED="false"><font BOLD="true" NAME="SansSerif" SIZE="12"/>
<node TEXT="Vad &#228;r det?">
<node TEXT="Data i tabeller (rader + kolumner)"/>
<node TEXT="Relationer mellan tabeller"/>
</node>
<node TEXT="N&#228;r anv&#228;nds den?">
<node TEXT="OLTP-system"/>
<node TEXT="M&#229;nga transaktioner"/>
<node TEXT="Komplexa fr&#229;gor"/>
<node TEXT="Insert, update, delete"/>
</node>
<node TEXT="Exempel">
<node TEXT="SQL Server"/>
<node TEXT="Oracle"/>
<node TEXT="MySQL"/>
<node TEXT="DB2"/>
</node>
</node>
<node TEXT="NoSQL - Columnar" POSITION="right" COLOR="#cc6600" FOLDED="false"><font BOLD="true" NAME="SansSerif" SIZE="12"/>
<node TEXT="Vad &#228;r det?">
<node TEXT="Lagrar data kolumnvis"/>
<node TEXT="V&#228;rden i samma kolumn lagras ihop"/>
</node>
<node TEXT="N&#228;r anv&#228;nds den?">
<node TEXT="Data warehouse och BI"/>
<node TEXT="Aggregeringar p&#229; kolumner"/>
<node TEXT="Enorma datam&#228;ngder"/>
<node TEXT="Effektiv komprimering"/>
</node>
<node TEXT="Exempel">
<node TEXT="Cassandra"/>
<node TEXT="HBase"/>
<node TEXT="Vertica"/>
<node TEXT="SAP HANA"/>
</node>
</node>
<node TEXT="NoSQL - Key-Value &amp; Document" POSITION="right" COLOR="#009933" FOLDED="false"><font BOLD="true" NAME="SansSerif" SIZE="12"/>
<node TEXT="Vad &#228;r det?">
<node TEXT="Key-Value: stor hashtabell, nyckel + v&#228;rde"/>
<node TEXT="Document: v&#228;rden som JSON/XML/BSON"/>
<node TEXT="Schemal&#246;s och flexibel"/>
</node>
<node TEXT="N&#228;r anv&#228;nds den?">
<node TEXT="Stora m&#228;ngder loggar och klickdata"/>
<node TEXT="Sociala medier"/>
<node TEXT="Schema som &#228;ndras ofta"/>
<node TEXT="Billig skalning (sharding)"/>
</node>
<node TEXT="Exempel">
<node TEXT="Key-Value: Riak, Amazon Dynamo"/>
<node TEXT="Document: MongoDB, Couchbase"/>
</node>
</node>
<node TEXT="NoSQL - Graph" POSITION="left" COLOR="#990099" FOLDED="false"><font BOLD="true" NAME="SansSerif" SIZE="12"/>
<node TEXT="Vad &#228;r det?">
<node TEXT="Noder (personer, saker)"/>
<node TEXT="Relationer mellan noder"/>
</node>
<node TEXT="N&#228;r anv&#228;nds den?">
<node TEXT="Starkt kopplad data"/>
<node TEXT="Rekommendationsmotorer"/>
<node TEXT="Bedr&#228;geriuppt&#228;ckt"/>
<node TEXT="Sociala n&#228;tverk"/>
</node>
<node TEXT="Exempel">
<node TEXT="Neo4j"/>
<node TEXT="FlockDB"/>
<node TEXT="InfiniteGraph"/>
</node>
</node>
<node TEXT="NewSQL" POSITION="left" COLOR="#cc0000" FOLDED="false"><font BOLD="true" NAME="SansSerif" SIZE="12"/>
<node TEXT="Vad &#228;r det?">
<node TEXT="Modern relationsdatabas"/>
<node TEXT="SQL + NoSQL-prestanda"/>
</node>
<node TEXT="N&#228;r anv&#228;nds den?">
<node TEXT="OLTP med stora volymer"/>
<node TEXT="Snabba transaktioner"/>
<node TEXT="M&#229;nga noder och datacenter"/>
</node>
<node TEXT="Exempel">
<node TEXT="VoltDB"/>
<node TEXT="NuoDB"/>
</node>
</node>
</node></map>