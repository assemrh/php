<?php

/**
 * Günlük döviz kurlarını okuma
 */
$zaman_asimi = time() - 600;
$url = 'https://www.tcmb.gov.tr/kurlar/today.xml';
$dosya = 'xml/kurlar.xml';
if (!file_exists($dosya) || filemtime($dosya) < $zaman_asimi) {
    file_put_contents($dosya, file_get_contents($url));
}
if (!$xml = simplexml_load_file($dosya)) {
    die('XML okuma hatası!');
}
echo '<table border="1" cellpadding="3" style="border-collapse:collapse">
<tr><th colspan="9">'.$xml['Tarih'].'</th></tr>
<tr>
    <th rowspan="2">Döviz</th>
    <th rowspan="2">Döviz Adı</th>
    <th rowspan="2">Birim</th>
    <th colspan="2">Döviz</th>
    <th colspan="2">Efektif</th>
    <th colspan="2">Çapraz Kur</th>
</tr>
<tr>
            <th>Alış</th>
            <th>Satış</th>
            <th>Alış</th>
            <th>Satış</th>
            <th>USD</th>
            <th>EURO</th>
        </tr>';
foreach ($xml->children() as $d) {
    if ($d['Kod'] == 'XDR') continue;
    echo <<<HTML_42645327425376
        <tr>
            <td><img src="https://www.tcmb.gov.tr/kurlar/kurlar_tr_dosyalar/images/{$d['Kod']}.gif">{$d['Kod']}</td>
            <td>{$d->Isim}</td>
            <td>{$d->Unit}</td>
            <td>{$d->ForexBuying}</td>
            <td>{$d->ForexSelling}</td>
            <td>{$d->BanknoteBuying}</td>
            <td>{$d->BanknoteSelling}</td>
            <td>{$d->CrossRateUSD}</td>
            <td>{$d->CrossRateOther}</td>
        </tr>
HTML_42645327425376;
}
echo '</table>';