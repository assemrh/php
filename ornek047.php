<?php

/**
 * Çerez'in değerini okuma
 */

if (isset($_COOKIE['ziyaretçi_no'])) {
    echo 'Merhaba ziyaretçi ' . $_COOKIE['ziyaretçi_no'];
} else {
    echo 'Merhaba yeni ziyaretçi';
}
