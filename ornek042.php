<form>
ID <input name="id"> <input type="submit" value="Bul">
</form>
<?php
/**
 * PDO ile veri döndüren sorgular: SELECT
 */
$id = isset($_GET['id']) ? $_GET['id'] : 0;
try {
    $db = new PDO('sqlite:veri/gakmyo.sqlite3');
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $db->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);

    $sql = 'select * from ogrenciler where id='.$id;
    $sorgu = $db->query($sql);
    if ($sorgu === false) {
        print('<br>Sorgu hatası:<pre>');
        print_r($db->errorInfo());
    } else {
        print '<table border="1"><tr><th>ID</th><th>No</th><th>Adı Soyadı</th><th>Vize</th><th>Final</th><th>Ortalama</th></tr>';
        while ($kayit = $sorgu->fetch()) {
            printf(
                '<tr><td>%d</td><td>%s<td>%s</td><td>%d</td><td>%d</td><td>%.1f</td></tr>',
                $kayit['id'],
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
