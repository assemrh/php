<?php

/**
 * XML oluşturma
 */

$dizi = array(
    array(
        'grup' => 'EĞİTİM',
        'baslik' => 'Öğretmen Olmak',
        'dil' => 'tr',
        'yazar' => 'Doğan Cüceloğlu, İrfan Erdoğan',
        'yil' => 2016,
        'fiyat' => 12,
        'doviz' => 'TL'
    ),
    array(
        'grup' => 'ROMAN',
        'baslik' => 'Çocukluk Adası',
        'dil' => 'tr',
        'yazar' => 'Karl Ove Knausgaard',
        'yil' => 2016,
        'fiyat' => '19.76',
        'doviz' => 'TL'
    ),
    array(
        'grup' => 'WEB',
        'baslik' => 'Learning PHP 7',
        'dil' => 'en-us',
        'yazar' => 'Antonio Lopez',
        'yil' => 2015,
        'fiyat' => '31.19',
        'doviz' => 'USD'
    ),
    array(
        'grup' => 'WEB',
        'baslik' => 'Learning XML',
        'dil' => 'en-us',
        'yazar' => 'Erik T. Ray',
        'yil' => 2003,
        'fiyat' => '39.95',
        'doviz' => 'USD'
    )
);

$xml = new SimpleXMLElement('<?xml version="1.0" encoding="UTF-8"?><kutuphane/>');
foreach($dizi as $k){
    $kitap = $xml->addChild('kitap');
    $kitap['grup'] = $k['grup'];
    $kitap->baslik = $k['baslik'];
    $kitap->baslik['dil'] = $k['dil'];
    $kitap->yazar = $k['yazar'];
    $kitap->yil = $k['yil'];
    $kitap->fiyat = $k['fiyat'];
    $kitap->fiyat['doviz'] = $k['doviz'];
}

$xml->asXML('xml/kitaplar.xml');
header('Content-Type: text/xml');
echo $xml->asXML();
