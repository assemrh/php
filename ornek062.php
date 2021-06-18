<?php

/**
 * XML oluşturma
 */

$xml = new SimpleXMLElement('<?xml version="1.0" encoding="UTF-8"?><posta/>');
$xml['dil'] = 'TR';
$kime = $xml->addChild('kime');
$kime->addChild('eposta', 'eposta1@site.com');
$kime->addChild('eposta', 'eposta2@site.com');
$kime->addChild('eposta', 'eposta3@site.com');
$xml->kimden = 'ben@sitem.com';
$xml->konu = 'Hatırlatma';
$xml->mesaj = 'XML çok kolaymış!';

$xml->asXML('xml/eposta.xml');
header('Content-Type: text/xml');
echo $xml->asXML();
