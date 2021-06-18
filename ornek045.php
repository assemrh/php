<form>
    Vize &gt;= <input name="v1"> <br>
    Vize &lt;= <input name="v2"> <br>
    <input type="submit" value="Bul">
</form>
<?php
/**
 * PDO ile Hazırlanmış (prepared) sorgular: SELECT
 */
$v1 = $_GET['v1'] ?? 0;
$v2 = $_GET['v2'] ?? 100;
try {
    $db = new PDO('sqlite:veri/gakmyo.sqlite3');
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $db->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);

    $sql = 'select *, (vize*0.4+final*0.6) as ort from ogrenciler where vize>=:V1 and vize<=:V2';
    $sorgu = $db->prepare($sql);
    $sorgu->bindValue(':V1', (int)$v1);
    $sorgu->bindValue(':V2', (int)$v2);
    $sorgu->execute();
    if ($sorgu === false) {
        print('<br>Sorgu hatası:<pre>');
        print_r($db->errorInfo());
    } else {
        print '<table border="1"><tr><th>ID</th><th>No</th><th>Adı Soyadı</th><th>Vize</th><th>Final</th><th>Ortalama</th></tr>';
        while ($kayit = $sorgu->fetch()) {
            printf(
                '<tr><td>%d</td><td>%s<td>%s</td><td>%d</td><td>%d</td><td>%.1f</td></tr>',
                $kayit['ort'],
                $kayit['no'],
                $kayit['adi'],
                $kayit['vize'],
                $kayit['final'],
                $kayit['vize'] * 0.4 + $kayit['final'] * 0.6
            );
        }
        echo '</table>';
    }
} catch (PDOException $e) {
    die('Bağlantı hatası: ' . $e->getMessage());
} catch (Exception $e) {
    die('Bilinmeyen hata: ' . $e->getMessage());
}
