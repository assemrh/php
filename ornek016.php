<?php
/*
    Dosya yükleme (file upload)
    Üyelik formu ile yüklenen dosyaların işlenmesi
*/
if (!isset($_POST['ad'])) {
    // sayfa ilk defa gösteriliyor
?>
    <form action="" method="POST" enctype="multipart/form-data">
        Kullanıcı Adı: <input type="text" name="ad" value="ali"><br>
        Şifre: <input type="password" name="sifre" value="123"><br>
        E-posta: <input type="email" name="eposta"><br>
        Cinsiyet:
        <input type="radio" name="cinsiyet" value="b" checked> Belirtilmemiş
        <input type="radio" name="cinsiyet" value="e"> Erkek
        <input type="radio" name="cinsiyet" value="k"> Kadın<br>
        Hobiler:
        <input type="checkbox" name="hobi[]" value="yuruyus"> Yürüyüş
        <input type="checkbox" name="hobi[]" value="okuma"> Kitap Okumak
        <input type="checkbox" name="hobi[]" value="program" checked> Programcılık<br>
        Eğitim:
        <select name="egitim">
            <option value="ilk">İlkokul</option>
            <option value="orta">Ortaokul</option>
            <option value="lise" selected>Lise</option>
            <option value="universite">Üniversite</option>
            <option value="yuksek">Yükseklisans</option>
            <option value="doktora">Doktora</option>
        </select><br>
        Adres:<br>
        <textarea name="adres" cols="40" rows="5"></textarea><br>
        Profil Resmi: <input type="file" name="resim"><br>
        <input type="submit" name="giris" value="Kaydet">
    </form>
<?php
} else {
    // form submit edilmiştir
    echo '<pre>';
    print_r($_POST);
    print_r($_FILES);
    // Yüklenen dosyayı ele alma
    if (isset($_FILES['resim'])) {
        if ($_FILES['resim']['error'] == UPLOAD_ERR_OK) {
            echo 'Yükleme başarılı';
            if (move_uploaded_file($_FILES['resim']['tmp_name'], 'resim/profil/' . $_POST['ad'] . '.' . pathinfo($_FILES['resim']['name'])['extension'])) {
                echo 'Profil resmi güncellendi...';
            } else {
                echo 'Profil resmi güncellenirken hata oluştu!';
            }
        } else if ($_FILES['resim']['error'] == UPLOAD_ERR_NO_FILE) {
            echo 'Profil resmi yüklenmemiş';
        }
    }
}
