<?php
/**
 * Exception (istisna) kullanımı
 * Kullanıcı tanımlı istisnalar
 */
class GecersizNotException extends Exception {
    public function __construct($kod=1001, Throwable $onceki = null)
    {
        parent::__construct('Notlar 0-100 arasında olmalı.', $kod, $onceki);
    }
    public function __toString()
    {
        return __CLASS__. ": ({$this->code}) : {$this->message}<br>";
    }
    public function bilgi(){
        return '<p>Bu istisna geçersiz not girilince oluşur.<p>';
    }
}
function Ortalama($v, $f){
    if(!is_numeric($v) || !is_numeric($f)){
        throw new InvalidArgumentException('Vize ve final sayısal olmalı.');
    }
    if($v<0 || $v>100){
        throw new GecersizNotException();
    }
    if($f<0 || $f>100){
        throw new GecersizNotException();
    }
    return $v*0.4 + $f*0.6;
}
// Kullanım örneği
$v = '50';
$f = -90;
try {
    printf('Vize=%d, Final=%d ise Ortalama=%.1f<br>', $v, $f, Ortalama($v, $f));
}
catch (GecersizNotException $hata){
    echo $hata;
    echo $hata->bilgi();
}
catch (Exception $hata){
    echo $hata->getMessage();
}
finally {
    echo '<h3>Programın sonu<h3>';
}