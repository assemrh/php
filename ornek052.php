<?php

/**
 * XML içeriğini okuma
 */

$s = "<?xml version='1.0' encoding='UTF-8'?>
<posta>
    <kime>sen@sitem.com</kime>
    <kimden>ben@sitem.com</kimden>
    <konu>Hatırlatma</konu>
    <mesaj>XML çok kolaymış!</mesaj>
</posta>";

$xml = simplexml_load_string($s);
echo $xml->kime.'<br>';
echo $xml->kimden.'<br>';
echo $xml->konu.'<br>';
echo $xml->mesaj.'<br>';
