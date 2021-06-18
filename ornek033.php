<style>
table {
    border-collapse: collapse;
}
th, td {
    padding: 3px 10px;
}
th {
    background-color: #000;
    color: #fff;
}
tr:nth-child(even) {
    background-color: #eee;
}
tr:hover {
    background-color: #bbb;
}
</style>
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
    echo '<table border="1"><tr><th colspan="3">Öğrenci Listesi</th></tr>';
    while($veri = mysqli_fetch_assoc($sonuc)){
        printf('<tr><td rowspan="6"><img src="resim/profil/%s" height="150"></td><th>ID</th><td>%d</td></tr>
            <tr><th>Öğrenci No</th><td>%s</td></tr>
            <tr><th>Adı Soyadı</th><td>%s</td></tr>
            <tr><th>Vize</th><td>%d</td></tr>
            <tr><th>Final</th><td>%d</td></tr>
            <tr><th>Ortalama</th><td>%.1f</td></tr>', 
            $veri['resim'] ? $veri['resim'] : 'yok.png',
            $veri['id'], $veri['ogr_no'], $veri['adi'], $veri['vize'], $veri['final'], 
            $veri['vize']*0.4+$veri['final']*0.6);
    }
    echo '</table>';
} else {
    echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
}
