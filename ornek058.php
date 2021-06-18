<?php

/**
 * XML attribute'ları (özellikler) okuma
 */

$dosya = 'xml/kutuphane.xml';
if(!file_exists($dosya)){
    die('XML dosyası bulunamadı!');
}
if(!$xml = simplexml_load_file($dosya)){
    die('XML okuma hatası!');
}
echo '<table border="1" cellpadding="3" style="border-collapse:collapse">
    <tr><th>Grup</th><th>Kitap Adı</th><th>Dil</th><th>Yazar</th><th>Yıl</th><th>Fiyat</th></tr>';
foreach($xml->children() as $kitap){
    printf('<tr><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%.2f %s</td></tr>', 
        $kitap['grup'], $kitap->baslik, $kitap->baslik['dil'], $kitap->yazar, $kitap->yil, $kitap->fiyat, $kitap->fiyat['doviz']);
}
echo '</table>';
