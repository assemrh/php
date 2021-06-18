<pre><?php
/*
    fopen(), fread(), fwrite(), fclose()
*/

$dosya = 'liste.txt';
$h = fopen($dosya, 'r');
// Dosyanın tamamını bir seferde okuma
// $veri = fread($h, filesize($dosya));
while($veri = fread($h, 100)){
    echo $veri;
}
flush();
fclose($h);
