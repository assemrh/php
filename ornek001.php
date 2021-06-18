<?php
/*
    Kare sınıfı ve Sihirli Metodlar (Magic Methods)
*/
class Kare
{
    var $kenar;
    function Alan()
    {
        return $this->kenar * $this->kenar;
    }

    function __construct($kenar)
    {
        $this->kenar = $kenar;
    }
    function __toString()
    {
        return sprintf('Kenarı % d olan karenin alanı %d olur<br>', $this->kenar, $this->Alan());
    }
}
$a = new Kare(5);
echo $a;

$b = new Kare(3);
echo $b;
