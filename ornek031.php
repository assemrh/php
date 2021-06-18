<?php
/**
 * Veritabanından kayıt okuma ve tablo olarak yazdırma
 */
if (!$db = mysqli_connect('localhost', 'root', '')) {
    die('Veritabanı sunucuya bağlantı hatası!');
}
if (!mysqli_select_db($db, 'gakmyo')) {
    die('Veritabanı seçim hatası.');
}
mysqli_set_charset($db, 'utf8');
if ($sonuc = mysqli_query($db, 'select * from ogrenciler')) {
    echo '<table border="1"><tr><th>ID</th><th>Öğrenci No</th><th>Adı Soyadı</th><th>Vize</th><th>Final</th><th>Ortalama</th></tr>';
    while($veri = mysqli_fetch_assoc($sonuc)){
        printf('<tr>
            <td>%d</td>
            <td>%s</td>
            <td>%s</td>
            <td>%d</td>
            <td>%d</td>
            <td>%.1f</td>
        </tr>', $veri['id'], $veri['ogr_no'], $veri['adi'], $veri['vize'], $veri['final'], $veri['vize']*0.4+$veri['final']*0.6);
    }
    echo '</table>';
} else {
    echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
}
