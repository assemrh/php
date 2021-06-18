<?php
/*
    Daire sınıfı, Alan metodu ve Sihirli Metodlar (Magic Methods)
*/
class Daire
{
    public $yaricap;
    public static $PI = 3.14;
    function Alan()
    {
        return self::$PI * $this->yaricap * $this->yaricap;
    }

    function __construct($yaricap)
    {
        $this->yaricap = $yaricap;
    }
    function __toString()
    {
        return sprintf('Yarıçapı % d olan dairenin alanı %f olur<br>', $this->yaricap, $this->Alan());
    }
}

$a = new Daire(5);
Daire::$PI = 3;
echo $a;

$b = new Daire(5);
Daire::$PI = M_PI;
echo $b;
echo $a;
